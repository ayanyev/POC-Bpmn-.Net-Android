package com.eazzyapps.example.android.obsolete

import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import io.ktor.client.*
import io.ktor.client.engine.okhttp.*
import io.ktor.client.features.websocket.*
import io.ktor.http.*
import io.ktor.http.cio.websocket.*
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.collect
import kotlinx.coroutines.flow.mapNotNull
import kotlinx.coroutines.flow.receiveAsFlow
import kotlinx.coroutines.launch

class KtorViewModel : ViewModel() {

    private val client = HttpClient(OkHttp) {
        install(WebSockets)
    }

    val name = arrayOf("Max", "Jorg", "Michael").random()

    val isOnShift = MutableStateFlow(false)

    val task = MutableStateFlow("")

    private var wsSession: DefaultClientWebSocketSession? = null

    init {

        viewModelScope.launch {

            client.webSocket(
                method = HttpMethod.Get,
                host = "10.0.2.2",
                port = 8080,
                path = "/picker/$name"
            ) {

                wsSession = this

                send("Shift request:$name:check")

                incoming.receiveAsFlow()
                    .mapNotNull { (it as? Frame.Text)?.readText() }
                    .collect { text ->
                        when(text.split(":")[0]) {
                            "Shift status" -> isOnShift.value = text.split(":")[1].toBoolean()
                            "Task" -> task.value = text.split(":")[1]
                        }
                        Log.d("Websocket", "Received: $text")
                    }

            }

        }

    }

    fun startShift() = viewModelScope.launch {
        wsSession?.send("Shift request:$name:true")
    }

    fun stopShift() = viewModelScope.launch {
        wsSession?.send("Shift request:$name:false")
    }

}
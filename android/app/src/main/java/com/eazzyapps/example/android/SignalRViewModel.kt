package com.eazzyapps.example.android

import android.util.Base64
import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.microsoft.signalr.HubConnection
import com.microsoft.signalr.HubConnectionBuilder
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.launch
import java.util.Base64.getEncoder

class SignalRViewModel : ViewModel() {

    val name = arrayOf("Max", /*"Jorg", "Michael"*/).random()

    private val credentials = Base64.encodeToString("$name:ertryrtytr".toByteArray(), Base64.NO_WRAP);

    private val hubConnection =
        HubConnectionBuilder
            .create("http://10.0.2.2:5000/pickershub")
            .withHeader("Authorization", "Basic $credentials")
            .build()

    val isOnShift = MutableStateFlow(false)

    val hasTask = MutableStateFlow(false)

    val task = MutableStateFlow("n/a")

    init {

        viewModelScope.launch {

            Log.d("ws", credentials)

            hubConnection.start()
                .subscribe(
                    { tryResumeShift() },
                    { Log.e("SignalR", it?.message ?: "Error connecting to hub") }
                )

            hubConnection.on("ShiftStartConfirmed") {
                isOnShift.value = true
            }

            hubConnection.on("ShiftStopConfirmed") {
                isOnShift.value = false
            }

            hubConnection.on(
                "TaskAssigned",
                { guid ->
                    task.value = guid
                    hasTask.value = true
                },
                String::class.java
            )

            hubConnection.on(
                "TaskRemoved",
                { guid ->
                    if (task.value == guid) {
                        task.value = "n/a"
                        hasTask.value = false
                    }
                },
                String::class.java
            )

        }

    }

    private fun tryResumeShift() = hubConnection.send("CheckActiveShift", name)

    fun startShift() = hubConnection.send("StartShift", name)

    fun stopShift() = hubConnection.send("StopShift", name)

    fun confirmTaskDone() = hubConnection.send("FinishTask", name)

}
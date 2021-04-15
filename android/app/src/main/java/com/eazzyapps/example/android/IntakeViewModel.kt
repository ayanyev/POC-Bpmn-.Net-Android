package com.eazzyapps.example.android

import android.util.Base64
import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.microsoft.signalr.HubConnectionBuilder
import com.microsoft.signalr.TypeReference
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.launch

class IntakeViewModel : ViewModel() {

    private val credentials = Base64.encodeToString("Maik:ertryrtytr".toByteArray(), Base64.NO_WRAP)

    private val hubConnection =
        HubConnectionBuilder
            .create("http://10.0.2.2:5000/intakedevicehub")
            .withHeader("Authorization", "Basic $credentials")
            .build()

    val name = arrayOf("Max", "Jorg", "Michael").random()

    val isProcessRunning = MutableStateFlow(false)

    init {

        viewModelScope.launch {

            Log.d("ws", credentials)

            hubConnection.start()
                .subscribe(
                    {
                        Log.d("SignalR", "Connected to the intake device hub")
                        startProcess()
                    },
                    { Log.e("SignalR", it?.message ?: "Error connecting to hub") }
                )

            hubConnection.on("ProcessStartConfirmed") {
                Log.d("SignalR", "Intake process started")
                isProcessRunning.value = true
                sendNoteId("note1")
            }

            hubConnection.on("ShiftStopConfirmed") {
                isProcessRunning.value = false
            }

            val typeRef = (object : TypeReference<Array<Article>>() {}).type

            hubConnection.on(
                "ArticlesListReceived",
                { articles ->
                    Log.d("SignalR", "$articles")
                },
                Articles::class.java
            )

        }

    }

    private fun tryResumeShift() = hubConnection.send("CheckActiveShift", name)

    fun startProcess() = hubConnection.send("StartIntakeProcess")

    fun stopProcess() = hubConnection.send("StopIntakeProcess")

    fun sendNoteId(noteId: String) = hubConnection.send("ProvideNoteId", noteId)

}
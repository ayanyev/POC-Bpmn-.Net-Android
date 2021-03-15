package com.eazzyapps.example.android

import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.microsoft.signalr.HubConnectionBuilder
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.launch

class SignalRViewModel : ViewModel() {

    private val hubConnection =
        HubConnectionBuilder
            .create("http://10.0.2.2:5000/pickershub")
            .build()

    val name = arrayOf("Max", "Jorg", "Michael").random()

    val isOnShift = MutableStateFlow(false)

    val hasTask = MutableStateFlow(false)

    val task = MutableStateFlow("n/a")

    init {

        viewModelScope.launch {

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
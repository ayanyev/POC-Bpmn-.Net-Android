package com.eazzyapps.example.android

import android.annotation.SuppressLint
import android.util.Base64
import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.eazzyapps.example.android.domain.SelectionOptions
import com.eazzyapps.example.android.domain.Task
import com.eazzyapps.example.android.domain.TaskCategory
import com.eazzyapps.example.android.domain.TaskCategory.Selection
import com.microsoft.signalr.HubConnectionBuilder
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.collect
import kotlinx.coroutines.launch

class IntakeViewModel : ViewModel() {

    val name = arrayOf("Max" /*"Jorg", "Michael"*/).random()

    private val credentials =
        Base64.encodeToString("$name:ertryrtytr".toByteArray(), Base64.NO_WRAP)

    private val hubConnection =
        HubConnectionBuilder
            .create("http://10.0.2.2:5000/intakedevicehub")
            .withHeader("Authorization", "Basic $credentials")
            .build()

    val isConnected = MutableStateFlow(false)

    val isProcessRunning = MutableStateFlow(false)

    val currentTask = MutableStateFlow(Task.default())

    init {

        viewModelScope.launch {

            hubConnection.on("ProcessStartConfirmed") {
                Log.d("SignalR", "Intake process started")
                isProcessRunning.value = true
            }

            hubConnection.on("ProcessStopConfirmed") {
                Log.d("SignalR", "Intake process stopped")
                isProcessRunning.value = false
            }

            hubConnection.on(
                "ArticlesListReceived",
                { articles ->
                    Log.d("SignalR", "$articles")
                },
                Articles::class.java
            )

            hubConnection.on(
                "DoInput",
                { taskId, key ->
                    Log.d("SignalR", "UserTask input: taskId = $taskId, key = $key")
                    currentTask.value = Task(TaskCategory.of(taskId, key), key)
                },
                String::class.java, String::class.java
            )

            hubConnection.on(
                "DoInputQuantity",
                { taskId, key ->
                    Log.d("SignalR", "UserTask input: taskId = $taskId, key = $key")
                    currentTask.value = Task(TaskCategory.of(taskId, key), key)
                },
                String::class.java, String::class.java
            )

            hubConnection.on(
                "DoInputSelection",
                { taskId, key, options ->
                    Log.d("SignalR", "UserTask input: taskId = $taskId, key = $key")
                    Log.d("SignalR", "options = $options")
                    currentTask.value = Task(Selection, key)
                },
                String::class.java, String::class.java, SelectionOptions::class.java
            )

        }

    }

    private fun tryResumeShift() = hubConnection.send("CheckActiveShift", name)

    @SuppressLint("CheckResult")
    fun connect() {
        hubConnection.start()
            .subscribe(
                {
                    isConnected.value = true
                    Log.d("SignalR", "Connected to the intake device hub")
                    Log.d("SignalR", "Used  credentials: $credentials")
                },
                { Log.e("SignalR", it?.message ?: "Error connecting to hub") }
            )
    }

    @SuppressLint("CheckResult")
    fun disconnect() {
        viewModelScope.launch {
            if (isProcessRunning.value) {
                stopProcess()
            }
            isProcessRunning.asStateFlow().collect {
                if (it) return@collect
                hubConnection.stop()
                    .subscribe(
                        {
                            isConnected.value = false
                            Log.d("SignalR", "Disconnected from the intake device hub")
                        },
                        { e -> Log.e("SignalR", e?.message ?: "Error disconnecting from hub") }
                    )
            }
        }
    }

    fun startProcess() = hubConnection.send("StartProcess")

    fun stopProcess() = hubConnection.send("StopProcess")

    fun sendInputData(map: Map<String, Any>) {
        Log.d("SignalR", "UserTask output: $map")
        hubConnection.send("SendInput", map)
    }

}
package com.eazzyapps.example.android

import android.util.Base64
import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.eazzyapps.example.android.IntakeViewModel.TaskCategory.*
import com.eazzyapps.example.android.domain.SelectionOptions
import com.microsoft.signalr.HubConnectionBuilder
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.launch

class IntakeViewModel : ViewModel() {

    sealed class TaskCategory(val text: String, val name: String) {
        object NoTask : TaskCategory("", "")
        class Input(taskId: String, text: String) : TaskCategory(text, taskId)
        object Scan : TaskCategory("Scan", "Intake.UT.Input.Scan")
        object Quantity : TaskCategory("Set quantity", "Intake.UT.Input.Quantity")
        object AdjustQuantity : TaskCategory("Adjust quantity", "Intake.UT.Input.Quantity.Adjust")
        object Selection : TaskCategory("Select", "Intake.UT.Input.Selection")

        companion object {

            fun of(category: String, key: String): TaskCategory = when {
                category.contains(Selection.name) -> Selection
                category.contains(Scan.name) -> Scan
                category.contains(AdjustQuantity.name) -> AdjustQuantity
                category.contains(Quantity.name) -> Quantity
                else -> Input(category, key)
            }

        }

    }

    class Task(
        val category: TaskCategory,
        private val valueKey: String
    ) {

        private val resultMap = mutableMapOf<String, Any>()

        init {

            if (category is Quantity || category is AdjustQuantity) {
                resultMap["forced_valid"] = category is AdjustQuantity
            }

        }

        fun toResult(result: Any): Map<String, Any> {
            resultMap.putAll(
                arrayOf(
                    "taskId" to category.name,
                    valueKey to result
                )
            )
            return resultMap
        }

        companion object {
            fun default() = Task(NoTask, "")
        }

    }

    val name = arrayOf("Max" /*"Jorg", "Michael"*/).random()

    private val credentials =
        Base64.encodeToString("$name:ertryrtytr".toByteArray(), Base64.NO_WRAP)

    private val hubConnection =
        HubConnectionBuilder
            .create("http://10.0.2.2:5000/intakedevicehub")
            .withHeader("Authorization", "Basic $credentials")
            .build()

    val isProcessRunning = MutableStateFlow(false)

    val currentTask = MutableStateFlow(Task.default())

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
            }

            hubConnection.on("ShiftStopConfirmed") {
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
                    Log.d("SignalR", "DoInputScan: $taskId, key = $key")
                    currentTask.value = Task(Companion.of(taskId, key), key)
                },
                String::class.java, String::class.java
            )

            hubConnection.on(
                "DoInputQuantity",
                { taskId, key ->
                    Log.d("SignalR", "DoInputQuantity: $taskId, key = $key")
                    currentTask.value = Task(Companion.of(taskId, key), key)
                },
                String::class.java, String::class.java
            )

            hubConnection.on(
                "DoInputSelection",
                { taskId, key, options ->
                    Log.d("SignalR", "DoInputSelection: $taskId, key = $key")
                    Log.d("SignalR", "options = $options")
                    currentTask.value = Task(Selection, key)
                },
                String::class.java, String::class.java, SelectionOptions::class.java
            )

        }

    }

    private fun tryResumeShift() = hubConnection.send("CheckActiveShift", name)

    fun startProcess() = hubConnection.send("StartIntakeProcess")

    fun stopProcess() = hubConnection.send("StopIntakeProcess")

    fun sendInputData(map: Map<String, Any>) {
        Log.d("SignalR", map.toString())
        hubConnection.send("SendInput", map)
    }

}
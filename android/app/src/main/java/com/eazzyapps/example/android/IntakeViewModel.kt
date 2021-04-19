package com.eazzyapps.example.android

import android.util.Base64
import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.eazzyapps.example.android.IntakeViewModel.Task.*
import com.eazzyapps.example.android.domain.SelectionOptions
import com.microsoft.signalr.HubConnectionBuilder
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.launch

class IntakeViewModel : ViewModel() {

    public sealed class Task(val text: String) {
        object NoTask : Task("")
        object Scan: Task("Scan")
        object Quantity : Task("Set quantity")
        object AdjustQuantity : Task("Adjust quantity")
        object Selection : Task("Select")
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

    val currentTask = MutableStateFlow<Task>(NoTask)

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

            hubConnection.on("DoInputQuantity") {
                currentTask.value = Selection
            }

            hubConnection.on("DoInputScan") {
                currentTask.value = Scan
            }

            hubConnection.on("DoInputQuantity",
                { isForced ->
                    currentTask.value = if (isForced) AdjustQuantity else Quantity
                },
                Boolean::class.java
            )

            hubConnection.on("DoInputSelection",
                { options ->
                    Log.d("SignalR", "$options")
                    currentTask.value = Selection
                },
                SelectionOptions::class.java
            )

        }

    }

    private fun tryResumeShift() = hubConnection.send("CheckActiveShift", name)

    fun startProcess() = hubConnection.send("StartIntakeProcess")

    fun stopProcess() = hubConnection.send("StopIntakeProcess")

    fun sendNoteId(noteId: String) = hubConnection.send("ProvideNoteId", noteId)

    fun sendScannedData(barcode: String) = hubConnection.send("SendScanResult", barcode)

    fun sendInputData(map: Map<String, Any>) = hubConnection.send("SendInput", map)

    fun sendQuantity(quantity: Int, isForced: Boolean = false) = sendInputData(
        mapOf(
            "quantity" to quantity,
            "forced_valid" to "$isForced"
        )
    )

}
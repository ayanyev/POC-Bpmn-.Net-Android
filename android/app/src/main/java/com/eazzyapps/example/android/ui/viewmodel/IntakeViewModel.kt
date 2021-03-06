package com.eazzyapps.example.android.ui.viewmodel

import android.annotation.SuppressLint
import android.util.Base64
import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.eazzyapps.example.android.BuildConfig
import com.eazzyapps.example.android.domain.*
import com.eazzyapps.example.android.ui.common.ActivityDelegate
import com.eazzyapps.example.android.ui.common.Message
import com.eazzyapps.example.android.ui.nav.Screen
import com.microsoft.signalr.HubConnection
import com.microsoft.signalr.HubConnectionBuilder
import com.microsoft.signalr.TypeReference
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.collect
import kotlinx.coroutines.launch
import org.koin.core.component.KoinComponent
import org.koin.core.component.inject

class IntakeViewModel : ViewModel(), KoinComponent {

    private val delegate by inject<ActivityDelegate>()

    private lateinit var hubConnection: HubConnection

    private lateinit var selectedName: String

    val names = listOf("Max", "Jorg", "Michael")

    val isLoggedIn = MutableStateFlow(false)

    val isConnected = MutableStateFlow(false)

    val isProcessRunning = MutableStateFlow(false)

    val screenTitle = MutableStateFlow("<<<  Log in")

    val currentTask = MutableStateFlow<Task<*>>(Task.default())

    val articleList = MutableStateFlow<List<Article>>(listOf())

    val isError = MutableStateFlow(false)

    fun setLogIn(index: Int) {
        selectedName = names[index]
        isLoggedIn.value = true
    }

    @SuppressLint("CheckResult")
    fun connect() {

        if (!::selectedName.isInitialized) throw Exception("Attempt to connect before logged in")

        val credentials =
            Base64.encodeToString("$selectedName:ertryrtytr".toByteArray(), Base64.NO_WRAP)

        hubConnection = HubConnectionBuilder
            .create("${BuildConfig.PROCESS_APP_URL}:5000/intakedevicehub") //10.0.11.185, 192.168.0.38
            .withHeader("Authorization", "Basic $credentials")
            .build()

        listenToHub()

        hubConnection.start()
            .subscribe(
                {
                    isConnected.value = true
                    screenTitle.value = "<<<  Start process"
                    Log.d("SignalR", "Connected to the intake device hub")
                    Log.d("SignalR", "Used credentials: $credentials")
                    Log.d("SignalR", "ConnectionId: ${hubConnection.connectionId}")
                },
                {
                    Log.e("SignalR", it?.message ?: "Error connecting to hub")
                }
            )
    }

    @SuppressLint("CheckResult")
    fun disconnect() {

        if (!::hubConnection.isInitialized) throw Exception("Hub connection not initialized")

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
                            screenTitle.value = "<<<  Log in"
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
        delegate.showLoading(true)
        hubConnection.send("SendInput", map)
    }

    private fun listenToHub() {

        viewModelScope.launch {

            hubConnection.on(
                "ProcessStartConfirmed",
                { name ->
                    Log.d("SignalR", "$name process started")
                    delegate.showLoading(false)
                    screenTitle.value = name
                    isProcessRunning.value = true
                },
                String::class.java
            )

            hubConnection.on("ProcessStopConfirmed") {
                Log.d("SignalR", "Intake process stopped")
                articleList.value = listOf()
                delegate.showLoading(false)
                isProcessRunning.value = false
                screenTitle.value = "<<<  Start process"
            }

            hubConnection.on(
                "ArticlesListReceived",
                { articles ->
                    Log.d("SignalR", "$articles")
                    // TODO maybe move this logic to BE
                    val isProcessComplete = articles.items.all { it.isCompleted }
                    articleList.value = if (!isProcessComplete) articles.items else listOf()
                },
                Articles::class.java
            )

            hubConnection.on(
                "DoInput",
                { task: Task<Any> ->
                    Log.d("SignalR", "Client input task received: $task")
                    navigate(task)
                    delegate.showLoading(false)
                    currentTask.value = task
                },
                (object : TypeReference<Task<Any>>() {}).type
            )

            hubConnection.on(
                "DoInputScan",
                { task: Task<ValidBarcodes> ->
                    Log.d("SignalR", "Client scanning task received: $task")
                    navigate(task)
                    delegate.showLoading(false)
                    currentTask.value = task
                },
                (object : TypeReference<Task<ValidBarcodes>>() {}).type
            )

            hubConnection.on(
                "DoInputSelection",
                { task: Task<SelectionOptions> ->
                    Log.d("SignalR", "Client selection task received: $task")
                    navigate(task)
                    delegate.showLoading(false)
                    currentTask.value = task
                },
                (object : TypeReference<Task<SelectionOptions>>() {}).type
            )

            hubConnection.on(
                "ShowInfo",
                { task: Task<String> ->
                    Log.d("SignalR", "Client info task received: $task")
                    delegate.showLoading(false)
                    navigate(task)
                    currentTask.value = task
                },
                (object : TypeReference<Task<String>>() {}).type
            )

        }

    }

    fun setInputError(value: Boolean) {
        isError.value = value
    }

    fun selectScannedItem(gtin: String) {
        val items = articleList.value
        items
            .onEach {
                if (it.isUnfinished) {
                    it.setState(Article.State.INITIAL)
                    if (it.gtin == gtin && !it.isSuspended)
                        it.setState(Article.State.SELECTED)
                }
            }
        articleList.value = items
    }

    private fun navigate(task: Task<*>) = when (task.category) {

        is TaskCategory.Input -> delegate.navigate(Screen.NoteIdInput)

        is TaskCategory.Scan -> delegate.navigate(Screen.Scan)

        is TaskCategory.Selection -> delegate.navigate(Screen.BundleSelection)

        is TaskCategory.Quantity -> delegate.navigate(Screen.QuantityAdjustment)

        is TaskCategory.Info -> {

            delegate.showMessage(Message.Dialog(
                title = "Warning",
                text = task.payload as String,
                buttonText = "Ok",
                onClick = {
                    sendInputData(currentTask.value.toResult(true))
                }
            ))

        }

        else -> {
        }
    }

}
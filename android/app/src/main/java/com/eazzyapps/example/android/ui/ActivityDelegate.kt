package com.eazzyapps.example.android.ui

import com.eazzyapps.example.android.Screen
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.delay
import kotlinx.coroutines.flow.MutableSharedFlow
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asSharedFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch


class ActivityDelegate : CoroutineScope {

    override val coroutineContext = CoroutineScope(Dispatchers.Main.immediate).coroutineContext

    // loading

    private val _loadingFlow = MutableStateFlow(false)

    val loadingFlow = _loadingFlow.asStateFlow()

    fun showLoading(isLoading: Boolean) {
        launch {
            throttleLoadingInterval(isLoading)
            _loadingFlow.value = isLoading
        }
    }

    private val minLoadingInterval = 1000L

    private var loadingStartTime = System.currentTimeMillis()

    private suspend fun throttleLoadingInterval(isLoading: Boolean) {
        if (isLoading) loadingStartTime = System.currentTimeMillis()
        else {
            val actualLoadingInterval = System.currentTimeMillis() - loadingStartTime
            delay(minLoadingInterval - actualLoadingInterval)
        }
    }

    // messaging

    private val _msgFlow = MutableSharedFlow<Message>()

    val msgFlow = _msgFlow.asSharedFlow()

    fun showMessage(msg: Message) {
        launch {
            _msgFlow.emit(msg)
        }
    }

    // navigating

    private val _navFlow = MutableSharedFlow<Screen>()

    val navFlow = _navFlow.asSharedFlow()

    fun navigate(event: Screen) {
        launch {
            _navFlow.emit(event)
        }
    }

    fun navigateBack() {
        navigate(Screen.Previous)
    }

}
package eu.durstexpress.modules.scanner.view

import androidx.annotation.CallSuper
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import eu.durstexpress.modules.scanner.BuildConfig
import eu.durstexpress.modules.scanner.Scanner
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.launch
import timber.log.Timber

abstract class ScannerViewModel(

    private val scanner: Scanner

) : ViewModel() {

    protected abstract val scanResultsObserver: Observer<String>

    open val inputHint: String = ""

    open val removeDots: Boolean = false

    open val noManualInput: Boolean = !BuildConfig.DEBUG

    open val showInput = MutableStateFlow(false)

    init {
        viewModelScope.launch {
            scanner.apply {
                enable()
            }
        }
    }

    fun toggleInput() {
        clearInput()
        showInput.value = !(showInput.value)
    }

    fun clearInput(){
        scanner.emitResult("")
    }

    fun isInputEnabled() = showInput.value

    @CallSuper
    open fun onWrongCodeScanned(errorMessage: String?) {
        errorMessage?.let { Timber.d("Wrong code scanned $errorMessage") }
        showInput.value = false
    }

    override fun onCleared() {
        scanner.disable()
        super.onCleared()
    }

    fun String.removeDots(): String =
        if (removeDots) replace(".", "") else this

}
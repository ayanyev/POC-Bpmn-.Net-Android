package com.eazzyapps.example.android.ui.viewmodel

import androidx.lifecycle.Observer
import androidx.lifecycle.viewModelScope
import eu.durstexpress.modules.scanner.Scanner
import eu.durstexpress.modules.scanner.view.ScannerViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.launch

class DexScannerViewModel(
    private val scanner: Scanner,
    val doOnItemScanned: (String) -> Unit
) : ScannerViewModel(scanner) {

    val input = MutableStateFlow("")

    override val inputHint: String = "Rolli-Barcode"

    override val scanResultsObserver: Observer<String> = Observer {
        input.value = it
        if (!isInputEnabled()) {
            submitInput(it)
//            doOnItemScanned.invoke(it)
        }
    }

    init {
        viewModelScope.launch {
            scanner.apply {
                observe(scanResultsObserver)
            }
        }
    }

    fun onInputSubmit(barcode: String) {
        submitInput(barcode)
        showInput.value = false
    }

    private fun submitInput(barcode: String) {
        clearInput()
        doOnItemScanned.invoke(barcode)
    }

    fun onInputChange(value: String) = scanner.emitResult(value)

}
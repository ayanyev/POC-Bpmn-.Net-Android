package com.eazzyapps.example.android.ui.viewmodel

import androidx.lifecycle.ViewModel
import kotlinx.coroutines.flow.MutableStateFlow

class QuantityAdjustmentViewModel(
    initialQuantity: Int = 0,
    val articleName: String,
    private val min: Int = 0,
    private val max: Int = 99,
    private val doOnConfirm: (Int) -> Unit = {}

) : ViewModel() {

    val quantity = MutableStateFlow(initialQuantity)

    private fun changeQuantity(direction: Int, step: Int) {
        quantity.value = (quantity.value + direction * step).adjustToRange()
    }

    fun increaseQuantity(step: Int) = changeQuantity(direction = 1, step = step)

    fun decreaseQuantity(step: Int) = changeQuantity(direction = -1, step = step)

    fun onConfirm() = doOnConfirm.invoke(quantity.value)

    private fun Int.adjustToRange(): Int = if (this < min) min else if (this > max) max else this
}
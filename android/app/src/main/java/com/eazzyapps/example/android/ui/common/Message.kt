package com.eazzyapps.example.android.ui.common

import android.content.Context
import androidx.annotation.StringRes

sealed class Message constructor(private val id: Int, private val formatArgs: List<Any>) {

    data class SnackBar(@StringRes val id: Int, val argsList: List<Any>) : Message(id, argsList) {
        constructor(@StringRes id: Int, vararg args: Any) : this(id, args.asList())
    }

    data class Dialog(
        val title: String,
        val text: String,
        val buttonText: String,
        val onClick: () -> Unit
    ) : Message(0, listOf()) {

        var doDismiss: () -> Unit = {}

    }

    fun getText(context: Context) = context.resources.getString(id, formatArgs)

}
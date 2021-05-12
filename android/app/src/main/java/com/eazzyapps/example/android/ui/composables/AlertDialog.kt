package com.eazzyapps.example.android.ui.composables

import androidx.compose.material.AlertDialog
import androidx.compose.material.Button
import androidx.compose.material.Text
import androidx.compose.runtime.*
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.window.DialogProperties

@Preview
@Composable
fun AlertDialogLayoutPreview() {
    AlertDialogLayout(title = "Hooray!", text = "We did it!", onConfirm = {})
}

@Composable
fun AlertDialogLayout(title: String, text: String, onConfirm: () -> Unit) {
    AlertDialog(
        onDismissRequest = { },
        title = { Text(title) },
        text = { Text(text) },
        confirmButton = {
            Button(
                onClick = {
                    onConfirm()
                }
            )
            { Text("Ok") }
        },
        properties = DialogProperties(
            dismissOnBackPress = false,
            dismissOnClickOutside = false
        )
    )
}
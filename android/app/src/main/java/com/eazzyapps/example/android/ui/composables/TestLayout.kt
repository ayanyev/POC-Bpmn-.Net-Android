package com.eazzyapps.example.android.ui.composables

import androidx.compose.foundation.layout.*
import androidx.compose.material.OutlinedButton
import androidx.compose.material.OutlinedTextField
import androidx.compose.material.Text
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp

@Preview
@Composable
fun ScanLayoutPreview() {
    TestLayout(
        modifier = Modifier
            .wrapContentHeight()
            .width(200.dp),
        initial = "345454",
        label = "Input",
        isEnabled = true,
        isError = true,
        onClick = {}
    )
}

@Composable
fun TestLayout(
    modifier: Modifier,
    initial: String,
    label: String,
    isEnabled: Boolean,
    isError: Boolean,
    onClick: (String) -> Unit
) {
    Row(
        verticalAlignment = Alignment.Bottom,
        modifier = modifier.wrapContentHeight()
    ) {

        var text by remember { mutableStateOf(initial) }

        OutlinedTextField(
            modifier = Modifier
                .wrapContentHeight()
                .fillMaxWidth(fraction = 0.6f),
            value = text,
            label = { Text(label) },
            enabled = isEnabled,
            isError = isError,
            onValueChange = { text = it }
        )

        Spacer(modifier = Modifier.width(8.dp))

        OutlinedButton(
            modifier = Modifier
                .fillMaxWidth()
                .height(56.dp),
            onClick = { onClick(text) },
            enabled = isEnabled
        ) {
            Text(
                textAlign = TextAlign.Center,
                text = "Send"
            )
        }
    }
}
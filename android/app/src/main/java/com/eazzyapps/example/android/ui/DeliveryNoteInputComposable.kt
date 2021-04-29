package com.eazzyapps.example.android.ui

import androidx.compose.foundation.layout.*
import androidx.compose.material.OutlinedButton
import androidx.compose.material.OutlinedTextField
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp


@Composable
fun DeliveryNoteInputComposable(
    onInputSubmit: (String) -> Unit,
    isError: Boolean = false
) {

    val input = rememberSaveable { mutableStateOf("") }

    Column(
        Modifier
            .padding(32.dp)
            .fillMaxWidth(),
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        OutlinedTextField(
            modifier = Modifier.fillMaxWidth(),
            value = input.value,
            onValueChange = { input.value = it },
            label = {
                Text("Enter Delivery Note id")
            },
            isError = isError
        )
        if (isError) {
            Spacer(Modifier.height(8.dp))
            Text(
                "Please enter a valid Delivery Note id",
                color = Color.Red,
                fontSize = 14.sp,
                modifier = Modifier.align(Alignment.Start)
            )
        }
        Spacer(Modifier.height(16.dp))
        OutlinedButton(onClick = {
            onInputSubmit.invoke(input.value)
        }) {
            Text("Submit")
        }
    }
}
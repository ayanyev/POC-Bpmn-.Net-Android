package com.eazzyapps.example.android

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import androidx.activity.compose.setContent
import androidx.compose.foundation.layout.*
import androidx.compose.material.OutlinedButton
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.lifecycle.viewmodel.compose.viewModel

class IntakeActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent { TextScreenUi() }
    }
}

@Composable
fun TextScreenUi() {

    val viewModel: IntakeViewModel = viewModel()

    val isRunning by viewModel.isProcessRunning.collectAsState()

    Box(modifier = Modifier.fillMaxSize()) {
        Column(
            modifier = Modifier
                .align(Alignment.Center)
                .fillMaxHeight()
                .width(150.dp),
            verticalArrangement = Arrangement.Center,
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            OutlinedButton(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                onClick = { viewModel.sendNoteId("note1") },
                enabled = isRunning
            ) {
                Text(
                    textAlign = TextAlign.Center,
                    text = "Grab articles"
                )
            }
            Spacer(modifier = Modifier.height(32.dp))
            OutlinedButton(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                onClick = { viewModel.sendScannedData("567.456.7.9") },
                enabled = isRunning
            ) {
                Text(
                    textAlign = TextAlign.Center,
                    text = "Scan article"
                )
            }
            Spacer(modifier = Modifier.height(32.dp))
            OutlinedButton(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                onClick = { viewModel.sendInputData(mapOf("bundleId" to 1)) },
                enabled = isRunning
            ) {
                Text(
                    textAlign = TextAlign.Center,
                    text = "Send bundle id"
                )
            }
            Spacer(modifier = Modifier.height(32.dp))
            OutlinedButton(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                onClick = { viewModel.sendQuantity(25, isForced = false) },
                enabled = isRunning
            ) {
                Text(
                    textAlign = TextAlign.Center,
                    text = "Send quantity"
                )
            }
            Spacer(modifier = Modifier.height(32.dp))
            OutlinedButton(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                onClick = { viewModel.sendQuantity(25, isForced = true) },
                enabled = isRunning
            ) {
                Text(
                    textAlign = TextAlign.Center,
                    text = "Adjust quantity"
                )
            }
            Spacer(modifier = Modifier.height(32.dp))
            OutlinedButton(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                onClick = { viewModel.sendScannedData("123.456.7.8") },
                enabled = isRunning
            ) {
                Text(
                    textAlign = TextAlign.Center,
                    text = "Scan location"
                )
            }
        }
    }
}
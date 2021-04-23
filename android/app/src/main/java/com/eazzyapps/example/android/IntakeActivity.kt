package com.eazzyapps.example.android

import android.os.Bundle
import android.util.Log
import androidx.activity.compose.setContent
import androidx.appcompat.app.AppCompatActivity
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
import androidx.lifecycle.viewmodel.compose.viewModel
import com.eazzyapps.example.android.domain.TaskCategory.*

class IntakeActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent { TextScreenUi() }
    }
}

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
            modifier = Modifier.fillMaxWidth().height(56.dp),
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

@Composable
fun TextScreenUi() {

    val space = 16.dp

    val viewModel: IntakeViewModel = viewModel()

    val isRunning by viewModel.isProcessRunning.collectAsState()

    val isConnected by viewModel.isConnected.collectAsState()

    val currentTask by viewModel.currentTask.collectAsState()

    Log.d("SignalR: Activity", "${currentTask.category}")

    Box(modifier = Modifier.fillMaxSize()) {
        Column(
            modifier = Modifier
                .align(Alignment.Center)
                .fillMaxHeight()
                .width(200.dp),
            verticalArrangement = Arrangement.Center,
            horizontalAlignment = Alignment.CenterHorizontally
        ) {

            OutlinedButton(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                onClick = { viewModel.connect() },
                enabled = !isConnected
            ) {
                Text(
                    textAlign = TextAlign.Center,
                    text = "Connect"
                )
            }

            Spacer(modifier = Modifier.height(space))

            OutlinedButton(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                onClick = { viewModel.startProcess() },
                enabled = isConnected && !isRunning
            ) {
                Text(
                    textAlign = TextAlign.Center,
                    text = "Start process"
                )
            }

            Spacer(modifier = Modifier.height(space))

            TestLayout(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                initial = "note1",
                label = if (currentTask.category is Simple) currentTask.label else "",
                isEnabled = isRunning && currentTask.category is Simple,
                isError = currentTask.hasError,
                onClick = {
                    viewModel.sendInputData(currentTask.toResult(it))
                }
            )

            Spacer(modifier = Modifier.height(space))

            TestLayout(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                initial = "567.456.7.9",
                label = if (currentTask.category is Scan) currentTask.label else "",
                isEnabled = isRunning && currentTask.category is Scan,
                isError = currentTask.hasError,
                onClick = {
                    viewModel.sendInputData(currentTask.toResult(it))
                }
            )

            Spacer(modifier = Modifier.height(space))

            TestLayout(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                initial = "1",
                label = if (currentTask.category is Selection) currentTask.label else "",
                isEnabled = isRunning && currentTask.category is Selection,
                isError = currentTask.hasError,
                onClick = {
                    viewModel.sendInputData(currentTask.toResult(it.toInt()))
                }
            )

            Spacer(modifier = Modifier.height(space))

            TestLayout(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                initial = "25",
                label = if (currentTask.category is AdjustQuantity || currentTask.category is Quantity) currentTask.label else "",
                isEnabled = isRunning && ((currentTask.category is Quantity) || (currentTask.category is AdjustQuantity)),
                isError = currentTask.hasError,
                onClick = {
                    viewModel.sendInputData(currentTask.toResult(it.toInt()))
                }
            )

            Spacer(modifier = Modifier.height(space))

            OutlinedButton(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                onClick = { viewModel.stopProcess() },
                enabled = isConnected && isRunning
            ) {
                Text(
                    textAlign = TextAlign.Center,
                    text = "Stop process"
                )
            }

            Spacer(modifier = Modifier.height(space))

            OutlinedButton(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                onClick = { viewModel.disconnect() },
                enabled = isConnected
            ) {
                Text(
                    textAlign = TextAlign.Center,
                    text = "Disconnect"
                )
            }

        }
    }
}
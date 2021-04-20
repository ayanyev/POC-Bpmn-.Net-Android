package com.eazzyapps.example.android

import android.os.Bundle
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
import com.eazzyapps.example.android.IntakeViewModel.TaskCategory
import com.eazzyapps.example.android.IntakeViewModel.TaskCategory.*

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
        category = IntakeViewModel.Task(Selection, "").category,
        isEnabled = true,
        onClick = {}
    )
}


@Composable
fun TestLayout(modifier: Modifier, initial: String, category: TaskCategory, isEnabled: Boolean, onClick: (String) -> Unit) {
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
            label = { Text(category.text) },
            enabled = isEnabled,
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

    val viewModel: IntakeViewModel = viewModel()

    val isRunning by viewModel.isProcessRunning.collectAsState()

    val currentTask by viewModel.currentTask.collectAsState()

    Box(modifier = Modifier.fillMaxSize()) {
        Column(
            modifier = Modifier
                .align(Alignment.Center)
                .fillMaxHeight()
                .width(200.dp),
            verticalArrangement = Arrangement.Center,
            horizontalAlignment = Alignment.CenterHorizontally
        ) {

            TestLayout(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                initial = "note1",
                category = currentTask.category,
                isEnabled = currentTask.category is Input,
                onClick = {
                    viewModel.sendInputData(currentTask.toResult(it))
                }
            )

            Spacer(modifier = Modifier.height(32.dp))

            TestLayout(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                initial = "567.456.7.9",
                category = currentTask.category,
                isEnabled = currentTask.category is Scan,
                onClick = {
                    viewModel.sendInputData(currentTask.toResult(it))
                }
            )

            Spacer(modifier = Modifier.height(32.dp))

            TestLayout(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                initial = "1",
                category = Selection,
                isEnabled = currentTask.category is Selection,
                onClick = {
                    viewModel.sendInputData(currentTask.toResult(it.toInt()))
                }
            )

            Spacer(modifier = Modifier.height(32.dp))

            TestLayout(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                initial = "25",
                category = if (currentTask.category is AdjustQuantity) AdjustQuantity else Quantity,
                isEnabled = (currentTask.category is Quantity) || (currentTask.category is AdjustQuantity),
                onClick = {
                    viewModel.sendInputData(currentTask.toResult(it.toInt()))
                }
            )

        }
    }
}
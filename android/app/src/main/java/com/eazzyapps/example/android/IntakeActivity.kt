package com.eazzyapps.example.android

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import androidx.activity.compose.setContent
import androidx.compose.foundation.layout.*
import androidx.compose.material.MaterialTheme
import androidx.compose.material.OutlinedButton
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
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

//    Box(modifier = Modifier.fillMaxSize()) {
//        Column(
//            modifier = Modifier
//                .fillMaxHeight()
//                .width(150.dp),
//            verticalArrangement = Arrangement.Center,
//            horizontalAlignment = Alignment.CenterHorizontally
//        ) {
//            Text(
//                text = "I am $name",
//                style = MaterialTheme.typography.h6,
//                textAlign = TextAlign.Center
//            )
//            Spacer(modifier = Modifier.height(32.dp))
//            OutlinedButton(
//                modifier = Modifier
//                    .wrapContentHeight()
//                    .fillMaxWidth(),
//                onClick = onStart,
//                enabled = !isOnShift && !hasTask
//            ) {
//                Text(
//                    textAlign = TextAlign.Center,
//                    text = "Start shift"
//                )
//            }
//
//        }
//    }
}
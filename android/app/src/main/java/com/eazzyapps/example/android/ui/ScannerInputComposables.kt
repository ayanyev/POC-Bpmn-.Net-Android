package com.eazzyapps.example.android.ui

import android.util.Log
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.text.BasicTextField
import androidx.compose.foundation.text.KeyboardActions
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material.ContentAlpha
import androidx.compose.material.Icon
import androidx.compose.material.IconButton
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.SideEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.remember
import androidx.compose.ui.Alignment
import androidx.compose.ui.ExperimentalComposeUiApi
import androidx.compose.ui.Modifier
import androidx.compose.ui.focus.FocusRequester
import androidx.compose.ui.focus.focusRequester
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalFocusManager
import androidx.compose.ui.platform.LocalSoftwareKeyboardController
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.TextStyle
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.ImeAction
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.eazzyapps.example.android.R
import com.eazzyapps.example.android.getViewModel
import com.eazzyapps.example.android.ui.theme.grey
import com.eazzyapps.example.android.ui.theme.typography
import com.eazzyapps.example.android.ui.viewmodel.DexScannerViewModel
import org.koin.core.parameter.parametersOf

@ExperimentalComposeUiApi
@Composable
fun ScannerInputComposable(
    hint : String,
    isError : Boolean,
    errorMsg : String,
    onItemScanned : (String) -> Unit = {}) {

    val viewModel = getViewModel<DexScannerViewModel> { parametersOf(onItemScanned) }

    val input = viewModel.input.collectAsState()

    val isInputEnabled = viewModel.showInput.collectAsState()

    val focusRequester = remember { FocusRequester() }

    val focusManager = LocalFocusManager.current

    val keyboardController = LocalSoftwareKeyboardController.current

    Column {
        Row(
            verticalAlignment = Alignment.CenterVertically
        ) {
            Icon(
                modifier = Modifier.weight(0.2f),
                painter = painterResource(R.drawable.ic_scan),
                contentDescription = null
            )
            Box(
                modifier = Modifier
                    .weight(1f)
            ) {
                if (viewModel.input.value.isEmpty() && !isInputEnabled.value) {
                    Text(
                        text = hint,
                        textAlign = TextAlign.Center,
                        color = grey.copy(alpha = ContentAlpha.medium),
                        fontSize = 24.sp,
                        modifier = Modifier
                            .fillMaxWidth()
                    )
                }
                BasicTextField(
                    modifier = Modifier
                        .fillMaxWidth()
                        .focusRequester(focusRequester),
                    value = input.value,
                    onValueChange = { viewModel.onInputChange(it) },
                    textStyle = TextStyle(
                        textAlign = TextAlign.Center,
                        fontWeight = FontWeight.Medium,
                        fontSize = 24.sp
                    ),
                    keyboardActions = KeyboardActions(
                        onSend = {
                            viewModel.onInputSubmit(input.value)
                        }
                    ),
                    keyboardOptions = KeyboardOptions(
                        keyboardType = KeyboardType.Number,
                        imeAction = ImeAction.Send
                    ),
                    enabled = isInputEnabled.value,
                    singleLine = true,
                )
            }
            IconButton(modifier = Modifier
                .weight(0.2f),
                onClick = { viewModel.toggleInput() }) {
                Icon(
                    painter = painterResource(R.drawable.ic_keyboard),
                    contentDescription = null
                )
            }

            SideEffect {
                when (isInputEnabled.value) {
                    true -> focusRequester.requestFocus()
                    false -> {
                        focusManager.clearFocus()
                        keyboardController?.hide()
                    }
                }
            }

        }
        if (isError) {
            Text(
                text = errorMsg,
                color = Color.Red,
                fontSize = 14.sp,
                modifier = Modifier.align(Alignment.Start).padding(horizontal = 16.dp)
            )
        }

    }
}

@Composable
fun ScanInfoComposable(
    title : String,
    availableBarcodes : List<String>
){
    Column(modifier = Modifier
        .fillMaxWidth()
        .padding(16.dp),
    horizontalAlignment = Alignment.CenterHorizontally)
    {
        Text(text = title, style = typography.h6)
        Spacer(modifier = Modifier.height(8.dp))
        Text (text = availableBarcodes.joinToString(), style = typography.subtitle1, textAlign = TextAlign.Center )
        Spacer(modifier = Modifier.height(8.dp))
    }

}
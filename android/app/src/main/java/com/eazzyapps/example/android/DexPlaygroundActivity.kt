package com.eazzyapps.example.android

import android.os.Bundle
import androidx.activity.compose.setContent
import androidx.appcompat.app.AppCompatActivity
import androidx.compose.foundation.layout.*
import androidx.compose.material.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.ExperimentalComposeUiApi
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import com.eazzyapps.example.android.ui.QuantityAdjustmentUi
import com.eazzyapps.example.android.ui.ScannerInputUi
import com.eazzyapps.example.android.ui.theme.*
import com.eazzyapps.example.android.ui.viewmodel.QuantityAdjustmentViewModel
import kotlinx.coroutines.launch

class DexPlaygroundActivity : AppCompatActivity() {
    @ExperimentalMaterialApi
    @ExperimentalComposeUiApi
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            AndroidTheme(content = { PlaygroundScreenUi() }

            )
        }
    }
}

@ExperimentalComposeUiApi
@ExperimentalMaterialApi
@Composable
fun PlaygroundScreenUi() {
    val state = rememberModalBottomSheetState(ModalBottomSheetValue.Hidden)
    val scope = rememberCoroutineScope()
    val viewModel = QuantityAdjustmentViewModel(articleName = "Coca Cola Pfand 0.10")

    ModalBottomSheetLayout(
        sheetState = state,
        sheetContent = {
            QuantityAdjustmentUi(viewModel, state)
        }
    ) {
        Column(
            modifier = Modifier
                .fillMaxSize(),
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            ScannerInputUi()
            Spacer(Modifier.height(20.dp))
            Button(onClick = { scope.launch { state.show() } }) {
                Text("Click to show sheet")
            }
        }
    }

}
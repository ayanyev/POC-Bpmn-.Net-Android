package com.eazzyapps.example.android.domain

data class SelectionOptions(
    val items: List<SelectionOption>
)

data class SelectionOption(
    val id: Int,
    val text: String
)

data class ValidBarcodes(
    val barcodes: List<String>?
)
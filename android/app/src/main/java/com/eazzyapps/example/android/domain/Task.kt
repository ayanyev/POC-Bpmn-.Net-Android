package com.eazzyapps.example.android.domain

sealed class TaskCategory {
    object NoTask : TaskCategory()
    object Simple : TaskCategory()
    object Scan : TaskCategory()
    object Quantity : TaskCategory()
    object AdjustQuantity : TaskCategory()
    object Selection : TaskCategory()

    companion object {
        fun of(category: String): TaskCategory = when (category) {
            "Selection" -> Selection
            "Scan" -> Scan
            "Quantity" -> Quantity
            "AdjustQuantity" -> AdjustQuantity
            "Simple" -> Simple
            else -> NoTask
        }
    }
}

data class TaskError(
    val code: String,
    val name: String,
    val message: String
)

data class Task<T>(
    val id: String,
    val type: String,
    val label: String,
    private val resultKey: String,
    private val resultTemplate: MutableMap<String, Any>,
    val error: TaskError?,
    val payload: T?
) {

    val hasError get() = error?.code != null

    val category get() = TaskCategory.of(type)

    fun toResult(result: Any): Map<String, Any> {
        resultTemplate[resultKey] = result
        return resultTemplate
    }

    companion object {
        fun default() = Task("", "", "", "", mutableMapOf(), null, null)
    }

}

sealed class ScanEvent(val id: String, val label: String, val errorMsg: String) {
    object LocationScan : ScanEvent(
        id = "UT.Input.Scan.Barcode.Location",
        label = "Article Location Barcodes",
        errorMsg = "Please enter valid location Barcode"
    )

    object ArticleScan : ScanEvent(
        id = "UT.Input.Scan.Barcode.Article",
        label = "Available Article Barcodes",
        errorMsg = "Please enter valid Article Barcode"
    )

    companion object {

        fun of(taskId: String) = when (taskId) {
            "UT.Input.Scan.Barcode.Article" -> ArticleScan
            "UT.Input.Scan.Barcode.Location" -> LocationScan
            else -> throw Exception("Unknown scan event !")
        }
    }
}
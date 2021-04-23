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
    private val resultTemplate: MutableMap<String, String>,
    val error: TaskError?,
    val payload: T?
) {

    val hasError get() = error?.code != null

    val category get() = TaskCategory.of(type)

    fun toResult(result: String): Map<String, Any> {
        resultTemplate[resultKey] = result
        return resultTemplate
    }

    companion object {
        fun default() = Task("", "", "", "", mutableMapOf(), null, null)
    }

}
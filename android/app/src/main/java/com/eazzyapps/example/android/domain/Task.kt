package com.eazzyapps.example.android.domain

sealed class TaskCategory(val text: String, val name: String) {
    object NoTask : TaskCategory("", "")
    class Input(taskId: String, text: String) : TaskCategory(text, taskId)
    object Scan : TaskCategory("Scan", "Intake.UT.Input.Scan")
    object Quantity : TaskCategory("Set quantity", "Intake.UT.Input.Quantity")
    object AdjustQuantity : TaskCategory("Adjust quantity", "Intake.UT.Input.Quantity.Adjust")
    object Selection : TaskCategory("Select", "Intake.UT.Input.Selection")

    companion object {

        fun of(category: String, key: String): TaskCategory = when {
            category.contains(Selection.name) -> Selection
            category.contains(Scan.name) -> Scan
            category.contains(AdjustQuantity.name) -> AdjustQuantity
            category.contains(Quantity.name) -> Quantity
            else -> Input(category, key)
        }

    }

}

class Task(
    val category: TaskCategory,
    private val valueKey: String
) {

    private val resultMap = mutableMapOf<String, Any>()

    init {

        if (category is TaskCategory.Quantity || category is TaskCategory.AdjustQuantity) {
            resultMap["forced_valid"] = category is TaskCategory.AdjustQuantity
        }

    }

    fun toResult(result: Any): Map<String, Any> {
        resultMap.putAll(
            arrayOf(
                "taskId" to category.name,
                valueKey to result
            )
        )
        return resultMap
    }

    companion object {
        fun default() = Task(TaskCategory.NoTask, "")
    }

}
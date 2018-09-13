using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Localization;

namespace Одиноко_проживающие
{
    public class MyRussionRadGridLocalizationProvider : RadGridLocalizationProvider
    {
        public static string TableElementText = "Нет данных для отображения";
        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                case RadGridStringId.ConditionalFormattingPleaseSelectValidCellValue: return "Please select valid cell value";
                case RadGridStringId.ConditionalFormattingPleaseSetValidCellValue: return "Please set a valid cell value";
                case RadGridStringId.ConditionalFormattingPleaseSetValidCellValues: return "Please set a valid cell values";
                case RadGridStringId.ConditionalFormattingPleaseSetValidExpression: return "Please set a valid expression";
                case RadGridStringId.ConditionalFormattingItem: return "Item";
                case RadGridStringId.ConditionalFormattingInvalidParameters: return "Invalid parameters";
                case RadGridStringId.FilterFunctionBetween: return "Между";
                case RadGridStringId.FilterFunctionContains: return "Содержит";
                case RadGridStringId.FilterFunctionDoesNotContain: return "Не содержит";
                case RadGridStringId.FilterFunctionEndsWith: return "Заканчивается";
                case RadGridStringId.FilterFunctionEqualTo: return "Равно";
                case RadGridStringId.FilterFunctionGreaterThan: return "Greater than";
                case RadGridStringId.FilterFunctionGreaterThanOrEqualTo: return "Greater than or equal to";
                case RadGridStringId.FilterFunctionIsEmpty: return "Пусто";
                case RadGridStringId.FilterFunctionIsNull: return "Пустое";
                case RadGridStringId.FilterFunctionLessThan: return "Меньше чем";
                case RadGridStringId.FilterFunctionLessThanOrEqualTo: return "Меньше или равно";
                case RadGridStringId.FilterFunctionNoFilter: return "Без фильтра";
                case RadGridStringId.FilterFunctionNotBetween: return "Not between";
                case RadGridStringId.FilterFunctionNotEqualTo: return "Не равен";
                case RadGridStringId.FilterFunctionNotIsEmpty: return "Is not empty";
                case RadGridStringId.FilterFunctionNotIsNull: return "Не пустой";
                case RadGridStringId.FilterFunctionStartsWith: return "Начинается с";
                case RadGridStringId.FilterFunctionCustom: return "Кастомный (Custom)";
                case RadGridStringId.FilterOperatorBetween: return "Между";
                case RadGridStringId.FilterOperatorContains: return "Содержит";
                case RadGridStringId.FilterOperatorDoesNotContain: return "Не содержит";
                case RadGridStringId.FilterOperatorEndsWith: return "EndsWith";
                case RadGridStringId.FilterOperatorEqualTo: return "Равно";
                case RadGridStringId.FilterOperatorGreaterThan: return "GreaterThan";
                case RadGridStringId.FilterOperatorGreaterThanOrEqualTo: return "GreaterThanOrEquals";
                case RadGridStringId.FilterOperatorIsEmpty: return "IsEmpty";
                case RadGridStringId.FilterOperatorIsNull: return "IsNull";
                case RadGridStringId.FilterOperatorLessThan: return "LessThan";
                case RadGridStringId.FilterOperatorLessThanOrEqualTo: return "LessThanOrEquals";
                case RadGridStringId.FilterOperatorNoFilter: return "Нет фильтра";
                case RadGridStringId.FilterOperatorNotBetween: return "NotBetween";
                case RadGridStringId.FilterOperatorNotEqualTo: return "Не равно";
                case RadGridStringId.FilterOperatorNotIsEmpty: return "Не пусто";
                case RadGridStringId.FilterOperatorNotIsNull: return "Не ноль";
                case RadGridStringId.FilterOperatorStartsWith: return "StartsWith";
                case RadGridStringId.FilterOperatorIsLike: return "Содержит";
                case RadGridStringId.FilterOperatorNotIsLike: return "Не содержит";
                case RadGridStringId.FilterOperatorIsContainedIn: return "ContainedIn";
                case RadGridStringId.FilterOperatorNotIsContainedIn: return "NotContainedIn";
                case RadGridStringId.FilterOperatorCustom: return "Кастомный (Custom)";
                case RadGridStringId.CustomFilterMenuItem: return "Кастомный (Custom)";
                case RadGridStringId.CustomFilterDialogCaption: return "Пользовательский фильтр [{0}]";
                case RadGridStringId.CustomFilterDialogLabel: return "Показать только те строки, значения которых:";
                case RadGridStringId.CustomFilterDialogRbAnd: return "И";
                case RadGridStringId.CustomFilterDialogRbOr: return "ИЛИ";
                case RadGridStringId.CustomFilterDialogBtnOk: return "OK";
                case RadGridStringId.CustomFilterDialogBtnCancel: return "Отмена";
                case RadGridStringId.CustomFilterDialogCheckBoxNot: return "НЕ";
                case RadGridStringId.CustomFilterDialogTrue: return "True";
                case RadGridStringId.CustomFilterDialogFalse: return "False";
                case RadGridStringId.FilterMenuBlanks: return "Пусто";
                case RadGridStringId.FilterMenuAvailableFilters: return "Доступные фильтры";
                case RadGridStringId.FilterMenuSearchBoxText: return "Поиск...";
                case RadGridStringId.FilterMenuClearFilters: return "Очистить фильтр";
                case RadGridStringId.FilterMenuButtonOK: return "OK";
                case RadGridStringId.FilterMenuButtonCancel: return "Отмена";
                case RadGridStringId.FilterMenuSelectionAll: return "(Выделить всё)";
                case RadGridStringId.FilterMenuSelectionAllSearched: return "Результаты поиска";
                case RadGridStringId.FilterMenuSelectionNull: return "Ноль";
                case RadGridStringId.FilterMenuSelectionNotNull: return "Не ноль";
                case RadGridStringId.FilterFunctionSelectedDates: return "Фильтр по датам:";
                case RadGridStringId.FilterFunctionToday: return "Сегодня";
                case RadGridStringId.FilterFunctionYesterday: return "Вчера";
                case RadGridStringId.FilterFunctionDuringLast7days: return "Последние 7 дней";
                case RadGridStringId.FilterLogicalOperatorAnd: return "И";
                case RadGridStringId.FilterLogicalOperatorOr: return "ИЛИ";
                case RadGridStringId.FilterCompositeNotOperator: return "NOT";
                case RadGridStringId.DeleteRowMenuItem: return "Delete Row";
                case RadGridStringId.SortAscendingMenuItem: return "Сортировать по возрастанию";
                case RadGridStringId.SortDescendingMenuItem: return "Сортировать по убыванию";
                case RadGridStringId.ClearSortingMenuItem: return "Очистить сортировку";
                case RadGridStringId.ConditionalFormattingMenuItem: return "Conditional Formatting";
                case RadGridStringId.GroupByThisColumnMenuItem: return "Group by this column";
                case RadGridStringId.UngroupThisColumn: return "Ungroup this column";
                case RadGridStringId.ColumnChooserMenuItem: return "Column Chooser";
                case RadGridStringId.HideMenuItem: return "Hide Column";
                case RadGridStringId.HideGroupMenuItem: return "Hide Group";
                case RadGridStringId.UnpinMenuItem: return "Unpin Column";
                case RadGridStringId.UnpinRowMenuItem: return "Unpin Row";
                case RadGridStringId.PinMenuItem: return "Pinned state";
                case RadGridStringId.PinAtLeftMenuItem: return "Pin at left";
                case RadGridStringId.PinAtRightMenuItem: return "Pin at right";
                case RadGridStringId.PinAtBottomMenuItem: return "Pin at bottom";
                case RadGridStringId.PinAtTopMenuItem: return "Pin at top";
                case RadGridStringId.BestFitMenuItem: return "Выровнять ширину";
                case RadGridStringId.PasteMenuItem: return "Вставить";
                case RadGridStringId.EditMenuItem: return "Изменить";
                case RadGridStringId.ClearValueMenuItem: return "Clear Value";
                case RadGridStringId.CopyMenuItem: return "Копировать";
                case RadGridStringId.CutMenuItem: return "Вырезать";
                case RadGridStringId.AddNewRowString: return "Нажмите здесь, чтобы добавить новую строку";
                case RadGridStringId.ConditionalFormattingSortAlphabetically: return "Сортировать колонки в алфавитном порядке";
                case RadGridStringId.ConditionalFormattingCaption: return "Conditional Formatting Rules Manager";
                case RadGridStringId.ConditionalFormattingLblColumn: return "Format only cells with";
                case RadGridStringId.ConditionalFormattingLblName: return "Rule name";
                case RadGridStringId.ConditionalFormattingLblType: return "Cell value";
                case RadGridStringId.ConditionalFormattingLblValue1: return "Value 1";
                case RadGridStringId.ConditionalFormattingLblValue2: return "Value 2";
                case RadGridStringId.ConditionalFormattingGrpConditions: return "Rules";
                case RadGridStringId.ConditionalFormattingGrpProperties: return "Rule Properties";
                case RadGridStringId.ConditionalFormattingChkApplyToRow: return "Apply this formatting to entire row";
                case RadGridStringId.ConditionalFormattingChkApplyOnSelectedRows: return "Apply this formatting if the row is selected";
                case RadGridStringId.ConditionalFormattingBtnAdd: return "Add new rule";
                case RadGridStringId.ConditionalFormattingBtnRemove: return "Remove";
                case RadGridStringId.ConditionalFormattingBtnOK: return "OK";
                case RadGridStringId.ConditionalFormattingBtnCancel: return "Отмена";
                case RadGridStringId.ConditionalFormattingBtnApply: return "Apply";
                case RadGridStringId.ConditionalFormattingRuleAppliesOn: return "Rule applies to";
                case RadGridStringId.ConditionalFormattingCondition: return "Condition";
                case RadGridStringId.ConditionalFormattingExpression: return "Expression";
                case RadGridStringId.ConditionalFormattingChooseOne: return "[Choose one]";
                case RadGridStringId.ConditionalFormattingEqualsTo: return "equals to [Value1]";
                case RadGridStringId.ConditionalFormattingIsNotEqualTo: return "is not equal to [Value1]";
                case RadGridStringId.ConditionalFormattingStartsWith: return "starts with [Value1]";
                case RadGridStringId.ConditionalFormattingEndsWith: return "ends with [Value1]";
                case RadGridStringId.ConditionalFormattingContains: return "contains [Value1]";
                case RadGridStringId.ConditionalFormattingDoesNotContain: return "does not contain [Value1]";
                case RadGridStringId.ConditionalFormattingIsGreaterThan: return "is greater than [Value1]";
                case RadGridStringId.ConditionalFormattingIsGreaterThanOrEqual: return "is greater than or equal [Value1]";
                case RadGridStringId.ConditionalFormattingIsLessThan: return "is less than [Value1]";
                case RadGridStringId.ConditionalFormattingIsLessThanOrEqual: return "is less than or equal to [Value1]";
                case RadGridStringId.ConditionalFormattingIsBetween: return "is between [Value1] and [Value2]";
                case RadGridStringId.ConditionalFormattingIsNotBetween: return "is not between [Value1] and [Value1]";
                case RadGridStringId.ConditionalFormattingLblFormat: return "Format";
                case RadGridStringId.ConditionalFormattingBtnExpression: return "Expression editor";
                case RadGridStringId.ConditionalFormattingTextBoxExpression: return "Expression";
                case RadGridStringId.ConditionalFormattingPropertyGridCaseSensitive: return "CaseSensitive";
                case RadGridStringId.ConditionalFormattingPropertyGridCellBackColor: return "CellBackColor";
                case RadGridStringId.ConditionalFormattingPropertyGridCellForeColor: return "CellForeColor";
                case RadGridStringId.ConditionalFormattingPropertyGridEnabled: return "Enabled";
                case RadGridStringId.ConditionalFormattingPropertyGridRowBackColor: return "RowBackColor";
                case RadGridStringId.ConditionalFormattingPropertyGridRowForeColor: return "RowForeColor";
                case RadGridStringId.ConditionalFormattingPropertyGridRowTextAlignment: return "RowTextAlignment";
                case RadGridStringId.ConditionalFormattingPropertyGridTextAlignment: return "TextAlignment";
                case RadGridStringId.ConditionalFormattingPropertyGridCaseSensitiveDescription: return "Determines whether case-sensitive comparisons will be made when evaluating string values.";
                case RadGridStringId.ConditionalFormattingPropertyGridCellBackColorDescription: return "Enter the background color to be used for the cell.";
                case RadGridStringId.ConditionalFormattingPropertyGridCellForeColorDescription: return "Enter the foreground color to be used for the cell.";
                case RadGridStringId.ConditionalFormattingPropertyGridEnabledDescription: return "Determines whether the condition is enabled (can be evaluated and applied).";
                case RadGridStringId.ConditionalFormattingPropertyGridRowBackColorDescription: return "Enter the background color to be used for the entire row.";
                case RadGridStringId.ConditionalFormattingPropertyGridRowForeColorDescription: return "Enter the foreground color to be used for the entire row.";
                case RadGridStringId.ConditionalFormattingPropertyGridRowTextAlignmentDescription: return "Enter the alignment to be used for the cell values, when ApplyToRow is true.";
                case RadGridStringId.ConditionalFormattingPropertyGridTextAlignmentDescription: return "Enter the alignment to be used for the cell values.";
                case RadGridStringId.ColumnChooserFormCaption: return "Column Chooser";
                case RadGridStringId.ColumnChooserFormMessage: return "Drag a column header from the\ngrid here to remove it from\nthe current view.";
                case RadGridStringId.GroupingPanelDefaultMessage: return "Drag a column here to group by this column.";
                case RadGridStringId.GroupingPanelHeader: return "Группировка:";
                case RadGridStringId.PagingPanelPagesLabel: return "Страница";
                case RadGridStringId.PagingPanelOfPagesLabel: return "of";
                case RadGridStringId.NoDataText: return "Нет данных для отображения";
                case RadGridStringId.CompositeFilterFormErrorCaption: return "Filter Error";
                case RadGridStringId.CompositeFilterFormInvalidFilter: return "The composite filter descriptor is not valid.";
                case RadGridStringId.ExpressionMenuItem: return "Expression";
                case RadGridStringId.ExpressionFormTitle: return "Expression Builder";
                case RadGridStringId.ExpressionFormFunctions: return "Functions";
                case RadGridStringId.ExpressionFormFunctionsText: return "Text";
                case RadGridStringId.ExpressionFormFunctionsAggregate: return "Aggregate";
                case RadGridStringId.ExpressionFormFunctionsDateTime: return "Date-Time";
                case RadGridStringId.ExpressionFormFunctionsLogical: return "Logical";
                case RadGridStringId.ExpressionFormFunctionsMath: return "Math";
                case RadGridStringId.ExpressionFormFunctionsOther: return "Other";
                case RadGridStringId.ExpressionFormOperators: return "Operators";
                case RadGridStringId.ExpressionFormConstants: return "Constants";
                case RadGridStringId.ExpressionFormFields: return "Fields";
                case RadGridStringId.ExpressionFormDescription: return "Description";
                case RadGridStringId.ExpressionFormResultPreview: return "Result preview";
                case RadGridStringId.ExpressionFormTooltipPlus: return "Plus";
                case RadGridStringId.ExpressionFormTooltipMinus: return "Minus";
                case RadGridStringId.ExpressionFormTooltipMultiply: return "Multiply";
                case RadGridStringId.ExpressionFormTooltipDivide: return "Divide";
                case RadGridStringId.ExpressionFormTooltipModulo: return "Modulo";
                case RadGridStringId.ExpressionFormTooltipEqual: return "Equal";
                case RadGridStringId.ExpressionFormTooltipNotEqual: return "Not Equal";
                case RadGridStringId.ExpressionFormTooltipLess: return "Less";
                case RadGridStringId.ExpressionFormTooltipLessOrEqual: return "Less Or Equal";
                case RadGridStringId.ExpressionFormTooltipGreaterOrEqual: return "Greater Or Equal";
                case RadGridStringId.ExpressionFormTooltipGreater: return "Greater";
                case RadGridStringId.ExpressionFormTooltipAnd: return "Logical \"AND\"";
                case RadGridStringId.ExpressionFormTooltipOr: return "Logical \"OR\"";
                case RadGridStringId.ExpressionFormTooltipNot: return "Logical \"NOT\"";
                case RadGridStringId.ExpressionFormAndButton: return string.Empty; //if empty, default button image is used
                case RadGridStringId.ExpressionFormOrButton: return string.Empty; //if empty, default button image is used
                case RadGridStringId.ExpressionFormNotButton: return string.Empty; //if empty, default button image is used
                case RadGridStringId.ExpressionFormOKButton: return "OK";
                case RadGridStringId.ExpressionFormCancelButton: return "Отмена";
                //case RadGridStringId.SearchRowChooseColumns: return "SearchRowChooseColumns";
                //case RadGridStringId.SearchRowSearchFromCurrentPosition: return "SearchRowSearchFromCurrentPosition";
                //case RadGridStringId.SearchRowMenuItemMasterTemplate: return "SearchRowMenuItemMasterTemplate";
                //case RadGridStringId.SearchRowMenuItemChildTemplates: return "SearchRowMenuItemChildTemplates";
                //case RadGridStringId.SearchRowMenuItemAllColumns: return "SearchRowMenuItemAllColumns";
                //case RadGridStringId.SearchRowTextBoxNullText: return "SearchRowTextBoxNullText";
                case RadGridStringId.SearchRowResultsOfLabel: return "SearchRowResultsOfLabel";
                case RadGridStringId.SearchRowMatchCase: return "Match case";
            }
            return string.Empty;
        }
    }

    public class MyRadMessageLocalizationProvider : RadMessageLocalizationProvider
    {
        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                case RadMessageStringID.AbortButton: return "Abbruch";
                case RadMessageStringID.CancelButton: return "Отмена";
                case RadMessageStringID.IgnoreButton: return "Ignorieren";
                case RadMessageStringID.NoButton: return "Нет";
                case RadMessageStringID.OKButton: return "OK";
                case RadMessageStringID.RetryButton: return "Wiederholung";
                case RadMessageStringID.YesButton: return "Да";
                case RadMessageStringID.DetailsButton: return "Детали";
                default:
                    return base.GetLocalizedString(id);
            }
        }
    }

    public class MyRussionCalendarLocalizationProvider : CalendarLocalizationProvider
    {
        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                case CalendarStringId.CalendarClearButton:
                    return "Очистить";
                case CalendarStringId.CalendarTodayButton:
                    return "Сегодня";
                default:
                    return base.GetLocalizedString(id);
            }
        }
    }
}

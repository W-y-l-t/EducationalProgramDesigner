namespace EducationalProgramDesigner.ResultTypes;

public abstract record ChangingFieldsResult
{
    private ChangingFieldsResult() { }

    public sealed record Success : ChangingFieldsResult;

    public sealed record EditorIsNotTheAuthor : ChangingFieldsResult;

    public sealed record MinimumScoreMustBeLessThenOneHundred : ChangingFieldsResult;
}
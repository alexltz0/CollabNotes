using FluentValidation;

namespace CollabNotes.Application.Notes.Commands.UpdateNoteContent;

public sealed class UpdateNoteContentCommandValidator : AbstractValidator<UpdateNoteContentCommand>
{
    public UpdateNoteContentCommandValidator()
    {
        RuleFor(x => x.NoteId).NotEmpty().WithMessage("NoteId is required.");
        RuleFor(x => x.EditedBy).NotEmpty().WithMessage("EditedBy is required.");
    }
}

using GroupDocs.Annotation.Models;
using GroupDocs.Annotation.Models.AnnotationModels;
using CleverConversion.Common.Annotation.Entity.Web;
using GroupDocs.Annotation.Options;
using System;

namespace CleverConversion.Common.Annotation.Annotator
{
    public class TextReplacementAnnotator : AbstractTextAnnotator
    {
        private ReplacementAnnotation replacementAnnotation;

        public TextReplacementAnnotator(AnnotationDataEntity annotationData, PageInfo pageInfo)
            : base(annotationData, pageInfo)
        {
            replacementAnnotation = new ReplacementAnnotation
            {
                Points = GetPoints(annotationData, pageInfo),
                TextToReplace = annotationData.Text
            };
        }

        public override AnnotationBase AnnotateWord()
        {
            replacementAnnotation = InitAnnotationBase(replacementAnnotation) as ReplacementAnnotation;
            //replacementAnnotation.FontSize = 12;
            replacementAnnotation.FontColor = 1;
            //replacementAnnotation.Opacity = 1;
            //replacementAnnotation.BackgroundColor = 123456;
            return replacementAnnotation;
        }

        public override AnnotationBase AnnotatePdf()
        {
            return AnnotateWord();
        }

        public override AnnotationBase AnnotateCells()
        {
            return AnnotateWord();
        }

        public override AnnotationBase AnnotateSlides()
        {
            return AnnotateWord();
        }

        public override AnnotationBase AnnotateImage()
        {
            throw new NotSupportedException(string.Format(Message, annotationData.Type));
        }

        public override AnnotationBase AnnotateDiagram()
        {
            throw new NotSupportedException(string.Format(Message, annotationData.Type));
        }

        protected override AnnotationType GetType()
        {
            return AnnotationType.TextReplacement;
        }
    }
}
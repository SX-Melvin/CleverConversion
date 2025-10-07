using GroupDocs.Annotation.Models;
using GroupDocs.Annotation.Models.AnnotationModels;
using CleverConversion.Common.Annotation.Entity.Web;
using GroupDocs.Annotation.Options;
using System;

namespace CleverConversion.Common.Annotation.Annotator
{
    public class TextFieldAnnotator : BaseAnnotator
    {
        private TextFieldAnnotation textFieldAnnotation;

        public TextFieldAnnotator(AnnotationDataEntity annotationData, PageInfo pageInfo)
            : base(annotationData, pageInfo)
        {
            textFieldAnnotation = new TextFieldAnnotation {
                Box = GetBox(),
                FontFamily = !string.IsNullOrEmpty(annotationData.Font) ? annotationData.Font : "Arial",
                FontColor = annotationData.FontColor,
                FontSize = annotationData.FontSize == 0 ? 12 : annotationData.FontSize,
                Text = annotationData.Text
            };
        }
        
        public override AnnotationBase AnnotateWord()
        {
            textFieldAnnotation = InitAnnotationBase(textFieldAnnotation) as TextFieldAnnotation;
            return textFieldAnnotation;
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
            return AnnotateWord();
        }

        public override AnnotationBase AnnotateDiagram()
        {
            return AnnotateWord();
        }

        protected override AnnotationType GetType()
        {
            return AnnotationType.TextField;
        }
    }
}
using CleverConversion.Common.Annotation.Entity.Web;
using System;
using GroupDocs.Annotation.Options;
using GroupDocs.Annotation.Models.AnnotationModels;
using GroupDocs.Annotation.Models;

namespace CleverConversion.Common.Annotation.Annotator
{
    public class WatermarkAnnotator : BaseAnnotator
    {
        private WatermarkAnnotation watermarkAnnotation;

        public WatermarkAnnotator(AnnotationDataEntity annotationData, PageInfo pageInfo)
            : base(annotationData, pageInfo)
        {
            watermarkAnnotation = new WatermarkAnnotation
            {
                Box = GetBox(),
                FontFamily = !string.IsNullOrEmpty(annotationData.Font) ? annotationData.Font : "Arial",
                FontColor = annotationData.FontColor,
                FontSize = annotationData.FontSize == 0 ? 12 : annotationData.FontSize,
                Text = annotationData.Text
            };
        }

        public override AnnotationBase AnnotateWord()
        {
            watermarkAnnotation = InitAnnotationBase(watermarkAnnotation) as WatermarkAnnotation;
            return watermarkAnnotation;
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
            throw new NotSupportedException(string.Format(Message, annotationData.Type));
        }

        protected override AnnotationType GetType()
        {
            return AnnotationType.Watermark;
        }
    }
}
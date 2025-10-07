using GroupDocs.Annotation.Models;
using CleverConversion.Common.Annotation.Entity.Web;
using System;

namespace CleverConversion.Common.Annotation.Annotator
{
    public class AnnotatorFactory
    {
        /// <summary>
        /// Create annotator instance depending on type of annotation
        /// </summary>
        /// <param name="annotationData">AnnotationDataEntity</param>
        /// <param name="pageInfo">PageInfo</param>
        /// <returns></returns>
        public static BaseAnnotator createAnnotator(AnnotationDataEntity annotationData, PageInfo pageInfo)
        {
            AnnotationDataEntity roundedAnnotationData = RoundCoordinates(annotationData);
            switch (roundedAnnotationData.Type)
            {
                case "textHighlight":
                    return new TextHighlightAnnotator(roundedAnnotationData, pageInfo);
                case "area":
                    return new AreaAnnotator(roundedAnnotationData, pageInfo);
                case "point":
                    return new PointAnnotator(roundedAnnotationData, pageInfo);
                case "textStrikeout":
                    return new TexStrikeoutAnnotator(roundedAnnotationData, pageInfo);
                case "polyline":
                    return new PolylineAnnotator(roundedAnnotationData, pageInfo);
                case "textField":
                    return new TextFieldAnnotator(roundedAnnotationData, pageInfo);
                case "watermark":
                    return new WatermarkAnnotator(roundedAnnotationData, pageInfo);
                case "textReplacement":
                    return new TextReplacementAnnotator(roundedAnnotationData, pageInfo);
                case "arrow":
                    return new ArrowAnnotator(roundedAnnotationData, pageInfo);
                case "textRedaction":
                    return new TextRedactionAnnotator(roundedAnnotationData, pageInfo);
                case "resourcesRedaction":
                    return new ResourceRedactionAnnotator(roundedAnnotationData, pageInfo);
                case "textUnderline":
                    return new TextUnderlineAnnotator(roundedAnnotationData, pageInfo);
                case "distance":
                    return new DistanceAnnotator(roundedAnnotationData, pageInfo);
                default:
                    throw new ArgumentNullException("Wrong annotation data without annotation type!");
            }
        }

        private static AnnotationDataEntity RoundCoordinates(AnnotationDataEntity annotationData)
        {
            annotationData.Height = (float)Math.Round(annotationData.Height, 0);
            annotationData.Left = (float)Math.Round(annotationData.Left, 0);
            annotationData.Top = (float)Math.Round(annotationData.Top, 0);
            annotationData.Width = (float)Math.Round(annotationData.Width, 0);
            return annotationData;
        }
    }
}
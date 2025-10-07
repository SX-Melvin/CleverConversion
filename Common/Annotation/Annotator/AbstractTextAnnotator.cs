using GroupDocs.Annotation.Models;
using CleverConversion.Common.Annotation.Entity.Web;
using System.Collections.Generic;

namespace CleverConversion.Common.Annotation.Annotator
{
    public abstract class AbstractTextAnnotator : BaseAnnotator
    {
        protected AbstractTextAnnotator(AnnotationDataEntity annotationData, PageInfo pageInfo)
            : base(annotationData, pageInfo)
        {

        }
        protected static List<Point> GetPoints(AnnotationDataEntity annotationData, PageInfo pageInfo)
        {
            return new List<Point>
                {
                    new Point(annotationData.Left, pageInfo.Height - annotationData.Top),
                    new Point(annotationData.Left + annotationData.Width, pageInfo.Height - annotationData.Top),
                    new Point(annotationData.Left, pageInfo.Height - annotationData.Top - annotationData.Height),
                    new Point(annotationData.Left + annotationData.Width, pageInfo.Height - annotationData.Top - annotationData.Top)
                };
        }

        protected static List<Point> GetPointsForImages(AnnotationDataEntity annotationData, PageInfo pageInfo)
        {
            return new List<Point>
                {
                    new Point(annotationData.Left, annotationData.Top + annotationData.Height),
                    new Point(annotationData.Left + annotationData.Width, annotationData.Top + annotationData.Height),
                    new Point(annotationData.Left, annotationData.Top),
                    new Point(annotationData.Left + annotationData.Width, annotationData.Top)
                };
        }
    }
}
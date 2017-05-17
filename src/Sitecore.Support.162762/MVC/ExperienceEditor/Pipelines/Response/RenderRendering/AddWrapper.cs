using System;
using Sitecore.Diagnostics;
using Sitecore.Mvc.ExperienceEditor.Presentation;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Sitecore.Support.MVC.ExperienceEditor.Pipelines.Response.RenderRendering
{
    public class AddWrapper : Mvc.ExperienceEditor.Pipelines.Response.RenderRendering.AddWrapper
    {
        public void Process(RenderRenderingArgs args)
        {

            if (args.Rendered || Context.Site == null || !Context.PageMode.IsExperienceEditorEditing)
            {
                return;
            }
            if (!string.IsNullOrEmpty(args.Rendering.DataSource) && args.PageContext.Item.Database.Name != "core" && args.PageContext.Item.Database.GetItem(args.Rendering.DataSource) == null)
            {
                Log.Warn(string.Format("'{0}' is not valid datasource.", args.Rendering.DataSource), this);
                args.AbortPipeline();
                return;
            }
            IMarker marker = this.GetMarker();
            if (marker == null)
            {
                return;
            }
            int num = args.Disposables.FindIndex((IDisposable x) => x.GetType() == typeof(Wrapper));
            if (num < 0)
            {
                num = 0;
            }
            args.Disposables.Insert(num, new Wrapper(args.Writer, marker));
        }
    }
}
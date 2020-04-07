using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SmartDocumentor.GenericPlugin.ReviewPlugin;
using SmartDocumentor.GenericPlugin.Editors;
using SmartDocumentor.GenericPlugin.Fields;
using System.Globalization;
using System.IO;
using SmartDocumentor.Common.Util;
using SmartDocumentor.GenericPlugin.Demo.Base;
using SmartDocumentor.GenericPlugin.Demo.CustomFields;
using SmartDocumentor.Common.Serialization;
using SmartDocumentor.GenericPlugin.Fields.Form;
using SmartDocumentor.Core.Schemas.Task;

namespace SmartDocumentor.GenericPlugin.Demo.ReviewPlugin
{
    public partial class GenericPlugin : BaseReviewPlugin
    {
        #region Properties

        protected override string ConfigFileName { get { return "SmartDocumentor.GenericPlugin.Fields.xml"; } }

        protected override string PluginId { get { return "Faturas"; } }

        protected override bool ShowVendorFinder { get { return false; } }

        protected override bool ShowVendorHeader { get { return false; } }

        protected override bool ShowReviewErrorMessageToUser { get { return true; } }

        protected override string ReviewErrorMessage { get { return "Valide os campos em erro!"; } }

        protected override string MandatoryFieldErrorMessage { get { return "Campo '{0}' é obrigatório"; } }

        protected override string PluginLoadErrorMessage { get { return "Erro no plugin. Por favor contacte o departamento de informática"; } }

        protected override bool ShowDeleteConfirmationMessage { get { return false; } }

        protected override string DeleteConfirmationMessage { get { return "Tem a certeza que deseja eliminar ?"; } }

        protected override bool RunOCROnSelectEntitiesByRegion { get { return false; } }

#if DEBUG
        protected override bool LearningActive { get { return false; } }
#else
        protected override bool LearningActive { get { return true; } }
#endif

        #endregion

        public GenericPlugin()
        {
        }

        #region Method

        public override void SetCustomProperties(Dictionary<string, object> customProperties)
        {
            base.SetCustomProperties(customProperties);

        }

        protected override ICaptureFieldEditor CreateCaptureField(Field field)
        {
            if (field.Name == Constants.Campos.CustomTable)
            {
                var customField = new CustomSingleColumnGrid
                {
                    Dock = DockStyle.Top,
                    Location = new Point(0, 0),
                    TabIndex = field.OrderId,
                    FieldDescription = field.Description,
                    FieldId = field.Id,
                    FieldLabel = field.Label,
                    FieldName = field.Name,
                    Visible = field.Visible,
                    Enabled = field.Enable,
                    Required = field.Required,
                    Text = field.Label
                };

                return customField;
            }
            else
            {
                return base.CreateCaptureField(field);
            }
        }

        protected override void CaptureFieldCreated(ICaptureFieldEditor captureField)
        {
            if (captureField.FieldName == Constants.Campos.CustomTable)
            {
                var grid = captureField as CustomSingleColumnGrid;
                grid.CellEndEdit += (s, e) =>
                {
                    grid.GridView.Focus();
                    grid.GridView.CurrentCell = grid.GridView[e.ColumnIndex, e.RowIndex];
                };
            }
            else
            {
                base.CaptureFieldCreated(captureField);
            }
        }

        public override bool IsFieldValid(ICaptureFieldEditor field, out string errorMessage)
        {
            if (field.FieldName == Constants.Campos.CustomTable)
            {
                var customSingleColumnGrid = field as CustomSingleColumnGrid;
                customSingleColumnGrid.GridViewEndEdit();

                var lines = new List<string>();

                foreach (DataGridViewRow item in customSingleColumnGrid.GridView.Rows)
                {
                    var tempValue = Convert.ToString(item.Cells[0].Value);

                    if (!string.IsNullOrWhiteSpace(tempValue))
                    {

                        lines.Add(tempValue);
                    }
                }

                customSingleColumnGrid.FieldValue = SerializationHelper.SerializeCompress(lines);

                errorMessage = string.Empty;
                return true;
            }
            else
            {
                return base.IsFieldValid(field, out errorMessage);
            }
        }

        public override void SetActiveContext(SDTask task)
        {
            base.SetActiveContext(task);

            task.SetPropertyValue("ReviewAction", "");

            if (this.FieldsMapByFieldName.ContainsKey(Constants.Campos.VendorVAT))
            {
                // Forçar o focus inicial no primeiro campo
                var field = this.FieldsMapByFieldName[Constants.Campos.VendorVAT] as SDTextBox;
                field.Focus();
                field.TextBox.Focus();
                SmartFieldUserControl_OnHighlightEntity(field, field.Entity);
            }
        }

        #endregion
    }
}

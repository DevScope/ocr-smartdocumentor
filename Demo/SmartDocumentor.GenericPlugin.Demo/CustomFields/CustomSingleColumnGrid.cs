using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SmartDocumentor.GenericPlugin.Editors;
using SmartDocumentor.ImageProcessing.Tools.UI;
using SmartDocumentor.Invoices.Common;
using SmartDocumentor.Common.Serialization;
using System.Xml;

namespace SmartDocumentor.GenericPlugin.Demo.CustomFields
{
    public partial class CustomSingleColumnGrid : UserControl, IDisposable, ICaptureFieldEditor
    {
        public event BeforeSetFieldValueEventHandler BeforeSetFieldValue;

        public new event EventHandler Leave;

        public new event EventHandler Enter;

        public event DataGridViewCellEventHandler CellEndEdit;

        public event EventHandler EntityUnLinked;

        public CustomSingleColumnGrid()
        {
            InitializeComponent();
            _fieldValue = string.Empty;
            IsFieldValueValid = true;
            _errorMessage = string.Empty;
        }

        public new string Text { get; set; }
        string _errorMessage;
        string _fieldValue;
        string _valueOnFocus;
        bool _isFieldValue;
        bool _hasChanges;
        bool _ignoreTextBoxTextChangedEvent;

        public bool SuspendOnValueChangeEvents { get; set; }

        public Control Control
        {
            get
            {
                return this;
            }
        }

        public DataGridView GridView
        {
            get
            {
                return this.gridView;
            }
        }

        public CaptureFieldDataType FieldDataType { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!IsFieldValueValid)
            {
                var l = gridView.Location;
                var s = gridView.Size;
                var r = new Rectangle(l, s);
                using (Pen p = new Pen(Color.Red, 2.0F))
                {
                    e.Graphics.DrawRectangle(p, r);
                }

                errorProvider.SetError(gridView, ErrorMessage);
            }
            else
            {
                errorProvider.Clear();
            }
        }

        public bool IsFieldValueValid
        {
            get { return _isFieldValue; }
            set
            {
                if (_isFieldValue == value)
                    return;

                _isFieldValue = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has changes.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has changes; otherwise, <c>false</c>.
        /// </value>
        public bool HasChanges
        {
            get
            {
                return _hasChanges;
            }
            private set
            {
                _hasChanges = value;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                if (string.IsNullOrEmpty(_errorMessage))
                    errorProvider.Clear();
                else
                    errorProvider.SetError(gridView, _errorMessage);

                this.Invalidate();
            }
        }

        public ExtractedEntity Entity { get; set; } = null;

        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets the field identifier.
        /// </summary>
        /// <value>
        /// The field identifier.
        /// </value>
        public int FieldId { get; set; }

        public string FieldLabel
        {
            get
            {
                return labelField.Text;
            }
            set
            {
                labelField.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the field description.
        /// </summary>
        /// <value>
        /// The field description.
        /// </value>
        public string FieldDescription { get; set; }

        public string FieldValue
        {
            get
            {
                return _fieldValue;
            }
            set
            {
                HasChanges = true;
                _fieldValue = value;
                this.LoadGrid();
            }
        }

        public bool FieldIsEmpty
        {
            get
            {
                return string.IsNullOrWhiteSpace(_fieldValue);
            }
        }

        /// <summary>
        /// Gets the initial field value.
        /// </summary>
        /// <value>
        /// The initial field value.
        /// </value>
        public string InitialFieldValue
        {
            get
            {
                return _valueOnFocus;
            }
        }

        public bool NoLearning { get; set; }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
        }

        public void SetFieldValueByHighlight(string fieldValue)
        {
            if (this.gridView.CurrentCell == null)
                return;

            var currentTextCell = this.gridView.CurrentCell.OwningColumn as DataGridViewTextBoxColumn;
            if (currentTextCell != null)
            {
                var maxInput = currentTextCell.MaxInputLength;
                fieldValue = fieldValue.Substring(0, fieldValue.Length > maxInput ? maxInput : fieldValue.Length);

                this.gridView.BeginEdit(true);
                this.gridView.CurrentCell.Value = fieldValue;

                this.gridView.NotifyCurrentCellDirty(true);
                this.gridView.RefreshEdit();

            }
        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="fieldValue">The text value.</param>
        /// <param name="setAsInitialValue">if set to <c>true</c> [set as initial value].</param>
        /// <param name="resetEntityImage">if set to <c>true</c> [reset entity image].</param>
        public void SetFieldValue(string fieldValue, bool setAsInitialValue = false, bool resetEntityImage = false)
        {
            if (fieldValue == _fieldValue)
                return;

            if (!SuspendOnValueChangeEvents && BeforeSetFieldValue != null)
            {
                var e = new SetFieldValueEventArgs
                {
                    Cancel = false,
                    FieldValue = fieldValue
                };
                BeforeSetFieldValue(this, e);
                if (e.Cancel)
                    return;
                fieldValue = e.FieldValue;
            }

            this.FieldValue = fieldValue;
            HasChanges = !setAsInitialValue;
            _ignoreTextBoxTextChangedEvent = true;
        }

        internal void GridViewEndEdit()
        {
            this.gridView.CurrentCell?.DataGridView?.EndEdit();
            this.gridView.CurrentCell = null;
            this.gridView.EndEdit();
        }

        public void ActivateCustomButton(CaptureFieldEditorCustomButton button, Action<ICaptureFieldEditor> onClick)
        {
        }

        public void SetEntity(ImageViewer imageViewer, ExtractedEntity entity, bool setAsInitialValue = false)
        {
            //if (entity != null && entity.Bounds.Width > 0 && entity.Bounds.Height > 0)
            //{
            //    Entity = entity.Clone();
            //}
            //else
            //{
            //Entity = null;
            //}

            //if (setAsInitialValue)
            //{
            //    HasChanges = false;
            //}
        }

        public void ProcessAcceleratorKey(KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        private void gridView_Leave(object sender, EventArgs e)
        {
            this.GridView.CurrentCell = null;
            Leave?.Invoke(this, e);
        }

        private void gridView_Enter(object sender, EventArgs e)
        {
            Enter?.Invoke(this, e);
        }

        private void gridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            CellEndEdit?.Invoke(sender, e);
        }

        private void LoadGrid()
        {
            this.gridView.Rows.Clear();

            if (!string.IsNullOrEmpty(this._fieldValue))
            {
                var data = SerializationHelper.DecompressDeserialize<List<string>>(this._fieldValue);

                foreach (var item in data)
                {
                    var row = new string[] { item };
                    gridView.Rows.Add(row);
                }
            }

            this.gridView.Refresh();

            this.GridView.CurrentCell = null;
        }
    }
}

// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data.Entity;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;
using Rock.Attribute;
using Rock.Web.UI;

namespace RockWeb.Blocks.Utility
{
    [LinkedPage("Detail Page")]
    [RockObsolete( "1.16.7" )]
    [Obsolete( "This block type has been deprecated." )]
    [DisplayName("KC-GroupList")]
    [Category( "Utility" )]
    [Description("lists all the active groups of type Small Group")]
    [Rock.SystemGuid.BlockTypeGuid( "E333D1CC-CB55-4E73-8568-41DAD296971C" )]

  
    public partial class KC_GroupList : RockBlock, ICustomGridColumns
    {
        #region Fields

        // used for private variables

        #endregion

        #region Properties

        // used for public / protected properties

        #endregion

        #region Base Control Methods

        //  overrides of the base RockBlock methods (i.e. OnInit, OnLoad)

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
            gGroups.GridRebind += gGroups_GridRebind;

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger(upnlContent);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {

            base.OnLoad(e);
            
            if ( !Page.IsPostBack )
            {
                BindGrid();
            }

           
        }

        #endregion

        #region Events

        // handlers called by the controls on your block

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            BindGrid(); 
        }

        /// <summary>
        /// Handles the GridRebind event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void gGroups_GridRebind( object sender, EventArgs e )
        {
            BindGrid();
        }


        protected void gGroups_RowSelected(object sender, Rock.Web.UI.Controls.RowEventArgs e)
        {
            NavigateToLinkedPage("DetailPage", "GroupId", (int)e.RowKeyId);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void BindGrid()
        {
            var rockContext = new RockContext();
            var groupService = new GroupService(rockContext);

            // sample query to display a small groups
            // Use AsNoTracking() since these records won't be modified, and therefore don't need to be tracked by the EF change tracker
            var query = groupService.Queryable()
                .Where(g => g.IsActive && g.GroupType.Name == "Small Group")
                .Select(g => new
                {
                    g.Id,
                    g.Name,
                    GroupTypeName = g.GroupType.Name,
                    g.IsActive
                });

            // sort the query based on the column that was selected to be sorted
            var sortProperty = gGroups.SortProperty;

            if (gGroups.AllowSorting && sortProperty != null)
            {
                query = query.Sort(sortProperty);
            }
            else
            {
                query = query.OrderBy(g => g.Name);
            }


            // set the datasource as a query. This allows the grid to only fetch the records that need to be shown based on the grid page and page size
            gGroups.SetLinqDataSource(query);
            gGroups.DataBind();

        }

        #endregion
    }
}
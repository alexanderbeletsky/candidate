﻿define(['Hogan', '../../../shared/BaseView', './SiteRowView', 'text!/scripts/templates/dashboard/sitesList.html'],
    function (Hogan, BaseView, SiteRowView, template) {

    var SitesListView = BaseView.extend({
        initialize: function (options) {
            _.bindAll(this);

            if (!(options && options.collection)) {
                throw 'SitesListView: collection is required';
            }

            this.collection = options.collection;

            this.bindTo(this.collection, 'add', this.onSiteAdded);
            this.bindTo(this.collection, 'remove', this.onSiteRemoved);
        },

        template: function (context) {
            return Hogan.compile(template).render(context);
        },

        onRender: function () {
            this.collection.each (function (site) {
                var siteRowView = new SiteRowView ({ model: site });
                this.appendSubview(siteRowView.render(), this.$('.candidate-sites'));
            }, this);
        },

        onSiteAdded: function (site) {
            var siteRowView = new SiteRowView ({ model: site });
            this.appendSubview(siteRowView.render(), this.$('.candidate-sites'));
        },

        onSiteRemoved: function (site, collection, options) {
            var siteRowView = _.find(this.subviews, function (view) {
                return view.model.cid === site.cid;
            });
            siteRowView.close();
            this.detachSubview(siteRowView);
        }
    });
    
    return SitesListView;
});
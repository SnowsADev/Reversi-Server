this["spa_templates"] = this["spa_templates"] || {};
this["spa_templates"]["templates"] = this["spa_templates"]["templates"] || {};
this["spa_templates"]["templates"]["afgelopen"] = Handlebars.template({"1":function(container,depth0,helpers,partials,data) {
    return "    <p>Helaas, je hebt verloren...</p>\r\n";
},"3":function(container,depth0,helpers,partials,data) {
    return "    <p>Gefeliciteerd! Je hebt gewonnen!</p>\r\n";
},"5":function(container,depth0,helpers,partials,data) {
    return "    <p>Je hebt gelijk gespeeld met je tegenstander.</p>\r\n";
},"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    var stack1, helper, alias1=depth0 != null ? depth0 : (container.nullContext || {}), alias2=container.hooks.helperMissing, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "<div class=\"afgelopen--"
    + container.escapeExpression(((helper = (helper = lookupProperty(helpers,"resultaat") || (depth0 != null ? lookupProperty(depth0,"resultaat") : depth0)) != null ? helper : alias2),(typeof helper === "function" ? helper.call(alias1,{"name":"resultaat","hash":{},"data":data,"loc":{"start":{"line":1,"column":23},"end":{"line":1,"column":36}}}) : helper)))
    + "\">\r\n  <h2>Het spel is afgelopen!</h2>\r\n"
    + ((stack1 = (lookupProperty(helpers,"isEqual")||(depth0 && lookupProperty(depth0,"isEqual"))||alias2).call(alias1,(depth0 != null ? lookupProperty(depth0,"resultaat") : depth0),0,{"name":"isEqual","hash":{},"fn":container.program(1, data, 0),"inverse":container.noop,"data":data,"loc":{"start":{"line":3,"column":2},"end":{"line":5,"column":14}}})) != null ? stack1 : "")
    + ((stack1 = (lookupProperty(helpers,"isEqual")||(depth0 && lookupProperty(depth0,"isEqual"))||alias2).call(alias1,(depth0 != null ? lookupProperty(depth0,"resultaat") : depth0),1,{"name":"isEqual","hash":{},"fn":container.program(3, data, 0),"inverse":container.noop,"data":data,"loc":{"start":{"line":6,"column":2},"end":{"line":8,"column":14}}})) != null ? stack1 : "")
    + ((stack1 = (lookupProperty(helpers,"isEqual")||(depth0 && lookupProperty(depth0,"isEqual"))||alias2).call(alias1,(depth0 != null ? lookupProperty(depth0,"resultaat") : depth0),2,{"name":"isEqual","hash":{},"fn":container.program(5, data, 0),"inverse":container.noop,"data":data,"loc":{"start":{"line":9,"column":2},"end":{"line":11,"column":14}}})) != null ? stack1 : "")
    + "    <p>Klik op de knop om terug te keren naar de lobby.</p>\r\n    <a href=\"/spellen\" class=\"button-link\">Terug naar lobby</a>\r\n</div>";
},"useData":true});
Handlebars.registerHelper('isEqual', function(arg1, arg2, options) {
    return (arg1 === arg2) ? options.fn(this) : options.inverse(this);
});
this["spa_templates"]["templates"]["artiekelen"] = Handlebars.template({"1":function(container,depth0,helpers,partials,data) {
    var helper, alias1=depth0 != null ? depth0 : (container.nullContext || {}), alias2=container.hooks.helperMissing, alias3="function", alias4=container.escapeExpression, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "    <div class=\"swiper-slide\">\r\n        <article>\r\n            <header>\r\n                <h4>"
    + alias4(((helper = (helper = lookupProperty(helpers,"title") || (depth0 != null ? lookupProperty(depth0,"title") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"title","hash":{},"data":data,"loc":{"start":{"line":6,"column":20},"end":{"line":6,"column":29}}}) : helper)))
    + "</h2>\r\n            </header>\r\n            <main>"
    + alias4(((helper = (helper = lookupProperty(helpers,"description") || (depth0 != null ? lookupProperty(depth0,"description") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"description","hash":{},"data":data,"loc":{"start":{"line":8,"column":18},"end":{"line":8,"column":33}}}) : helper)))
    + "</main>\r\n            <footer><a href=\""
    + alias4(((helper = (helper = lookupProperty(helpers,"url") || (depth0 != null ? lookupProperty(depth0,"url") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"url","hash":{},"data":data,"loc":{"start":{"line":9,"column":29},"end":{"line":9,"column":36}}}) : helper)))
    + "\">lees meer</a></footer>\r\n        </article>\r\n    </div>\r\n";
},"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    var stack1, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "<div class=\"swiper-wrapper\">\r\n"
    + ((stack1 = lookupProperty(helpers,"each").call(depth0 != null ? depth0 : (container.nullContext || {}),(depth0 != null ? lookupProperty(depth0,"articles") : depth0),{"name":"each","hash":{},"fn":container.program(1, data, 0),"inverse":container.noop,"data":data,"loc":{"start":{"line":2,"column":4},"end":{"line":12,"column":13}}})) != null ? stack1 : "")
    + "</div>\r\n<div class=\"swiper-pagination\"></div>";
},"useData":true});
Handlebars.registerHelper("log", function(value) {
    console.debug(value);
});
Handlebars.registerPartial("fiche", Handlebars.template({"1":function(container,depth0,helpers,partials,data) {
    return "    <div class=\"bord__fiche--wit\"></div>\r\n";
},"3":function(container,depth0,helpers,partials,data) {
    return "    <div class=\"bord__fiche--zwart\"></div>\r\n";
},"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    var stack1, alias1=depth0 != null ? depth0 : (container.nullContext || {}), alias2=container.hooks.helperMissing, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return ((stack1 = (lookupProperty(helpers,"isEqual")||(depth0 && lookupProperty(depth0,"isEqual"))||alias2).call(alias1,depth0,1,{"name":"isEqual","hash":{},"fn":container.program(1, data, 0),"inverse":container.noop,"data":data,"loc":{"start":{"line":1,"column":0},"end":{"line":3,"column":12}}})) != null ? stack1 : "")
    + ((stack1 = (lookupProperty(helpers,"isEqual")||(depth0 && lookupProperty(depth0,"isEqual"))||alias2).call(alias1,depth0,2,{"name":"isEqual","hash":{},"fn":container.program(3, data, 0),"inverse":container.noop,"data":data,"loc":{"start":{"line":4,"column":0},"end":{"line":6,"column":12}}})) != null ? stack1 : "");
},"useData":true}));
this["spa_templates"]["templates"]["notificatie"] = Handlebars.template({"1":function(container,depth0,helpers,partials,data) {
    var helper, alias1=depth0 != null ? depth0 : (container.nullContext || {}), alias2=container.hooks.helperMissing, alias3="function", alias4=container.escapeExpression, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "<div class=\"popup popup--state-success\" data-spelId=\""
    + alias4(((helper = (helper = lookupProperty(helpers,"spelId") || (depth0 != null ? lookupProperty(depth0,"spelId") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"spelId","hash":{},"data":data,"loc":{"start":{"line":2,"column":53},"end":{"line":2,"column":63}}}) : helper)))
    + "\" data-spelerId=\""
    + alias4(((helper = (helper = lookupProperty(helpers,"spelerId") || (depth0 != null ? lookupProperty(depth0,"spelerId") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"spelerId","hash":{},"data":data,"loc":{"start":{"line":2,"column":80},"end":{"line":2,"column":92}}}) : helper)))
    + "\">\r\n    <header class=\"popup__header\">\r\n        <input class=\"popup__button--close\" type=\"button\" value=\"X\"/>\r\n    </header>\r\n    <main class=\"popup__main\">\r\n        <img class=\"popup__icon\" src=\""
    + alias4(((helper = (helper = lookupProperty(helpers,"image") || (depth0 != null ? lookupProperty(depth0,"image") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"image","hash":{},"data":data,"loc":{"start":{"line":7,"column":38},"end":{"line":7,"column":47}}}) : helper)))
    + "\">\r\n        <span>"
    + alias4(((helper = (helper = lookupProperty(helpers,"content") || (depth0 != null ? lookupProperty(depth0,"content") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"content","hash":{},"data":data,"loc":{"start":{"line":8,"column":14},"end":{"line":8,"column":25}}}) : helper)))
    + "</span>\r\n    </main>\r\n    <footer class=\"popup__footer\">\r\n        <input type=\"button\" value=\"Akkoort\" class=\"popup__btn--accept is-active\"/>\r\n        <input type=\"button\" value=\"Weigeren\" class=\"popup__btn--decline\" />\r\n    </footer>\r\n</div>\r\n";
},"3":function(container,depth0,helpers,partials,data) {
    var helper, alias1=depth0 != null ? depth0 : (container.nullContext || {}), alias2=container.hooks.helperMissing, alias3="function", alias4=container.escapeExpression, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "<div class=\"popup popup--state-warning\">\r\n    <header class=\"popup__header\">\r\n        <input class=\"popup__button--close\" type=\"button\" value=\"X\"/>\r\n    </header>\r\n    <main class=\"popup__main\">\r\n        <img class=\"popup__icon\" src=\""
    + alias4(((helper = (helper = lookupProperty(helpers,"image") || (depth0 != null ? lookupProperty(depth0,"image") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"image","hash":{},"data":data,"loc":{"start":{"line":21,"column":38},"end":{"line":21,"column":47}}}) : helper)))
    + "\">\r\n        <span>"
    + alias4(((helper = (helper = lookupProperty(helpers,"content") || (depth0 != null ? lookupProperty(depth0,"content") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"content","hash":{},"data":data,"loc":{"start":{"line":22,"column":14},"end":{"line":22,"column":25}}}) : helper)))
    + "</span>\r\n    </main>\r\n</div>\r\n";
},"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    var stack1, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return ((stack1 = lookupProperty(helpers,"if").call(depth0 != null ? depth0 : (container.nullContext || {}),(depth0 != null ? lookupProperty(depth0,"success") : depth0),{"name":"if","hash":{},"fn":container.program(1, data, 0),"inverse":container.program(3, data, 0),"data":data,"loc":{"start":{"line":1,"column":0},"end":{"line":25,"column":7}}})) != null ? stack1 : "");
},"useData":true});
this["spa_templates"]["templates"]["speelbord"] = Handlebars.template({"1":function(container,depth0,helpers,partials,data) {
    return "--disabled";
},"3":function(container,depth0,helpers,partials,data) {
    var stack1, helper, alias1=depth0 != null ? depth0 : (container.nullContext || {}), lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "                <tr scope=\"row\" id=\""
    + container.escapeExpression(((helper = (helper = lookupProperty(helpers,"index") || (data && lookupProperty(data,"index"))) != null ? helper : container.hooks.helperMissing),(typeof helper === "function" ? helper.call(alias1,{"name":"index","hash":{},"data":data,"loc":{"start":{"line":5,"column":36},"end":{"line":5,"column":46}}}) : helper)))
    + "\">\r\n"
    + ((stack1 = lookupProperty(helpers,"each").call(alias1,depth0,{"name":"each","hash":{},"fn":container.program(4, data, 0),"inverse":container.noop,"data":data,"loc":{"start":{"line":6,"column":20},"end":{"line":10,"column":29}}})) != null ? stack1 : "")
    + "                </tr>\r\n";
},"4":function(container,depth0,helpers,partials,data) {
    var stack1, helper, alias1=container.escapeExpression, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "                        <td bezetDoor=\""
    + alias1(container.lambda(depth0, depth0))
    + "\" id=\""
    + alias1(((helper = (helper = lookupProperty(helpers,"index") || (data && lookupProperty(data,"index"))) != null ? helper : container.hooks.helperMissing),(typeof helper === "function" ? helper.call(depth0 != null ? depth0 : (container.nullContext || {}),{"name":"index","hash":{},"data":data,"loc":{"start":{"line":7,"column":53},"end":{"line":7,"column":63}}}) : helper)))
    + "\">\r\n"
    + ((stack1 = container.invokePartial(lookupProperty(partials,"fiche"),depth0,{"name":"fiche","data":data,"indent":"                            ","helpers":helpers,"partials":partials,"decorators":container.decorators})) != null ? stack1 : "")
    + "                        </td>\r\n";
},"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    var stack1, alias1=depth0 != null ? depth0 : (container.nullContext || {}), lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "<div class=\"bord__wrapper"
    + ((stack1 = lookupProperty(helpers,"if").call(alias1,(depth0 != null ? lookupProperty(depth0,"disabled") : depth0),{"name":"if","hash":{},"fn":container.program(1, data, 0),"inverse":container.noop,"data":data,"loc":{"start":{"line":1,"column":25},"end":{"line":1,"column":58}}})) != null ? stack1 : "")
    + "\">\r\n    <table id=\"bord__table\" class=\"table table-bordered\">\r\n        <tbody>\r\n"
    + ((stack1 = lookupProperty(helpers,"each").call(alias1,(depth0 != null ? lookupProperty(depth0,"bord") : depth0),{"name":"each","hash":{},"fn":container.program(3, data, 0),"inverse":container.noop,"data":data,"loc":{"start":{"line":4,"column":12},"end":{"line":12,"column":21}}})) != null ? stack1 : "")
    + "        </tbody>\r\n    </table>\r\n</div>";
},"usePartial":true,"useData":true});
this["spa_templates"]["templates"]["stats"] = Handlebars.template({"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    var helper, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "<canvas id=\""
    + container.escapeExpression(((helper = (helper = lookupProperty(helpers,"id") || (depth0 != null ? lookupProperty(depth0,"id") : depth0)) != null ? helper : container.hooks.helperMissing),(typeof helper === "function" ? helper.call(depth0 != null ? depth0 : (container.nullContext || {}),{"name":"id","hash":{},"data":data,"loc":{"start":{"line":1,"column":12},"end":{"line":1,"column":18}}}) : helper)))
    + "\"></canvas>";
},"useData":true});
this["spa_templates"]["templates"]["feedbackWidget"] = this["spa_templates"]["templates"]["feedbackWidget"] || {};
this["spa_templates"]["templates"]["feedbackWidget"]["body"] = Handlebars.template({"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    var helper, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "<section class=\"body\">\r\n "
    + container.escapeExpression(((helper = (helper = lookupProperty(helpers,"bericht") || (depth0 != null ? lookupProperty(depth0,"bericht") : depth0)) != null ? helper : container.hooks.helperMissing),(typeof helper === "function" ? helper.call(depth0 != null ? depth0 : (container.nullContext || {}),{"name":"bericht","hash":{},"data":data,"loc":{"start":{"line":2,"column":1},"end":{"line":2,"column":12}}}) : helper)))
    + "\r\n </section>";
},"useData":true});
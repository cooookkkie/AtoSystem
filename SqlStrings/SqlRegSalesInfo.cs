using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlStrings
{
    public class SqlRegSalesInfo
    {  
        public StringBuilder RegSalesInfoCountSelect(RegSalesInfoSelectModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT");
            qry.Append($"  COUNT(*)");
            qry.Append($"  FROM reg_salesInfo");
            qry.Append($"  WHERE 1=1");
            if (model.prod_code != "" && model.prod_code != null)
                qry.Append($" AND prod_code LIKE '%@prod_code%'");
            if (model.prod_name != "" && model.prod_name != null)
                qry.Append($" AND prod_name LIKE '%@prod_name%'");
            if (model.origin != "" && model.origin != null)
                qry.Append($" AND origin LIKE '%@origin%'");
            if (model.prod_weight != "" && model.prod_weight != null)
                qry.Append($" AND prod_weight LIKE '%@prod_weight%'");
            if (model.prod_size != "" && model.prod_size != null)
                qry.Append($" AND prod_size LIKE '%@prod_size%'");
            if (model.warehouse != "" && model.warehouse != null)
                qry.Append($" AND warehouse LIKE '%@warehouse%'");
            if (model.PurchaseCo != "" && model.PurchaseCo != null)
                qry.Append($" AND PurchaseCo LIKE '%@PurchaseCo%'");
            if (model.manager_name != "" && model.manager_name != null)
                qry.Append($" AND manager_name LIKE '%@manager_name%'");
            if (model.sttdate != "" && model.sttdate != null)
                qry.Append($" AND editDate >= '@sttdate'");
            if (model.enddate != "" && model.enddate != null)
                qry.Append($" AND editDate <= '@enddate'");
            qry.Append($"  ORDER BY prod_code, prod_name, prod_weight, prod_size, price_purchase=0, CAST(price_purchase AS UNSIGNED)   ");

            return qry;
        }
        public StringBuilder RegSalesInfoSelect(RegSalesInfoSelectModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT                                                                                                       ");
            qry.Append($"   id                                                                                                         ");
            qry.Append($" , prod_code                                                                                                  ");
            qry.Append($" , prod_name                                                                                                  ");
            qry.Append($" , origin                                                                                                     ");
            qry.Append($" , prod_weight                                                                                                ");
            qry.Append($" , prod_size                                                                                                  ");
            qry.Append($" , price_sale                                                                                                 ");
            qry.Append($" , TRIM(price_purchase) AS price_purchase                                                                     ");
            qry.Append($" , PurchaseCo                                                                                                 ");
            qry.Append($" , warehouse                                                                                                  ");
            qry.Append($" , editDate                                                                                                   ");
            qry.Append($" , note1                                                                                                      ");
            qry.Append($" , note2                                                                                                      ");
            qry.Append($" , note3                                                                                                      ");
            qry.Append($" , CONCAT(IF(price_sale_last IS NOT NULL, IF(price_sale > price_sale_last,'▲','▼'),'') , ' ' , price_sale_last) AS price_sale_last      ");
            //qry.Append($" , price_sale_last                                                                                            ");
            qry.Append($" , price_sale_editDate                                                                                        ");
            qry.Append($" , CONCAT(IF(price_purchase_last IS NOT NULL, IF(price_purchase > price_purchase_last,'▲','▼'),'') , ' ' , price_purchase_last) AS price_purchase_last      ");
            //qry.Append($" , price_purchase_last                                                                                        ");
            qry.Append($" , price_purchase_editDate                                                                                    ");
            qry.Append($" , last_edit_user_name                                                                                        ");
            qry.Append($" , manager_name                                                                                               ");
            qry.Append($" , manager_name2                                                                                              ");
            qry.Append($"  FROM reg_salesInfo                                                                                          ");
            qry.Append($"  WHERE 1=1");
            if (model.prod_code != "" && model.prod_code != null)
                qry.Append($" AND prod_code LIKE '%@prod_code%'");
            if (model.prod_name != "" && model.prod_name != null)
                qry.Append($" AND prod_name LIKE '%@prod_name%'");
            if (model.origin != "" && model.origin != null)
                qry.Append($" AND origin LIKE '%@origin%'");
            if (model.prod_weight != "" && model.prod_weight != null)
                qry.Append($" AND prod_weight LIKE '%@prod_weight%'");
            if (model.prod_size != "" && model.prod_size != null)
                qry.Append($" AND prod_size LIKE '%@prod_size%'");
            if (model.warehouse != "" && model.warehouse != null)
                qry.Append($" AND warehouse LIKE '%@warehouse%'");
            if (model.PurchaseCo != "" && model.PurchaseCo != null)
                qry.Append($" AND PurchaseCo LIKE '%@PurchaseCo%'");
            if (model.manager_name != "" && model.manager_name != null)
                qry.Append($" AND manager_name LIKE '%@manager_name%'");
            if (model.sttdate != "" && model.sttdate != null)
                qry.Append($" AND editDate >= '@sttdate'");
            if (model.enddate != "" && model.enddate != null)
                qry.Append($" AND editDate <= '@enddate'");
            qry.Append($"  ORDER BY prod_code, prod_name, prod_weight, prod_size, price_purchase=0, CAST(price_purchase AS UNSIGNED)   ");

            return qry;
        }
}

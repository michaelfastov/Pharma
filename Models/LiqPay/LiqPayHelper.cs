﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pharma.Models.LiqPay
{
	public class LiqPayHelper
    {
        static private readonly string _private_key;
        static private readonly string _public_key;

        static LiqPayHelper()
        {
            _public_key = "i20223994800";     // Public Key компанії, який можна знайти в особистому кабінеті на сайті liqpay.ua
            _private_key = "BGWV9rgDTLgkCr1PxFo9bTIGeFSvt8lTzLBgjEsb";    // Private Key компанії, який можна знайти в особистому кабінеті на сайті liqpay.ua
        }

        /// <summary>
        /// Сформувати дані для LiqPay (data, signature)
        /// </summary>
        /// <param name="order_id">Номер замовлення</param>
        /// <returns></returns>
        static public LiqPayCheckoutFormModel GetLiqPayModel(string order_id, int amount)
        {
            // Заповнюю дані для їх передачі для LiqPay
            var signature_source = new LiqPayCheckout()
            {
                public_key = _public_key,
                version = 3,
                action = "pay",
                amount = amount,
                currency = "UAH",
                description = "Оплата замовлення",
                order_id = order_id,
                sandbox = 1,

                result_url = "https://localhost:44321/api/Receptions/UpdatePayedReception",
            };
            var json_string = JsonConvert.SerializeObject(signature_source);
            var data_hash = Convert.ToBase64String(Encoding.UTF8.GetBytes(json_string));
            var signature_hash = GetLiqPaySignature(data_hash);

            // Данні для передачі у в'ю
            var model = new LiqPayCheckoutFormModel();
            model.Data = data_hash;
            model.Signature = signature_hash;
            return model;
        }

        /// <summary>
        /// Формування сигнатури
        /// </summary>
        /// <param name="data">Json string з параметрами для LiqPay</param>
        /// <returns></returns>
        static public string GetLiqPaySignature(string data)
        {
            return Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(_private_key + data + _private_key)));
        }
    }
}

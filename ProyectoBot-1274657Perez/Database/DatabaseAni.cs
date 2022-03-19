using ProyectoBot_1274657Perez.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoBot_1274657Perez.Database
{
    public class DatabaseAni
{
        public static List<AniModels> GetAni()
        {
            var lista = new List<AniModels>()
            {
                new AniModels()
                {
                    nombre = "Onepiece",
                     fecha= "2022",
                    imagen = "https://i1.sndcdn.com/artworks-000118285096-mk56yf-t500x500.jpg",
                    informacion = "https://onepiece.fandom.com/es/wiki/One_Piece_Blue:_Grand_Data_File",
                },
                new AniModels()
                {
                    nombre = "Anohana",
                    fecha = "2021",
                    imagen = "https://i1.sndcdn.com/artworks-000242902276-ysmork-t500x500.jpg",
                    informacion = "https://myanimelist.net/anime/9989/Ano_Hi_Mita_Hana_no_Namae_wo_Bokutachi_wa_Mada_Shiranai",
                },
                new AniModels()
                {
                    nombre = "Joze to tora to sakana-tachi",
                    fecha = "2019",
                    imagen = "https://images2-mega.cdn.mdstrm.com/etcetera/2020/10/13/13169_1_5f862bd74b40b.jpg?d=500x500",
                    informacion = "https://www.crunchyroll.com/es/anime-news/2020/12/24-1/la-pelcula-josee-to-tora-to-sakana-tachi-muestra-sus-primeros-cuatro-minutos",
                },
                 new AniModels()
                {
                    nombre = "Cop craft",
                    fecha = "2019",
                    imagen = "https://i1.sndcdn.com/artworks-0BgGJOxkS8mx94pj-urhGiQ-t500x500.jpg",
                    informacion = "",
                }
            };
            return lista;
        }
        public static List<AniPayments> GetPayment()
        {
            var lista2 = new List<AniPayments>()
            {
                new AniPayments()
                {
                    nombre = "PAYPAL",
                    imagen = "https://www.arcointeractiva.com/wp-content/uploads/2017/06/logo-paypal.png",
                },
                new AniPayments()
                {
                    nombre = "Visa",
                    imagen = "https://www.donregalo.pe/app/view/front-end/last/imgs/icon-visa.jpg",
                },
                new AniPayments()
                {
                    nombre = "Mercado pago",
                    imagen = "https://i0.wp.com/consultarse.org/wp-content/uploads/2014/08/version-vertical-mercadopag.jpg",
                },
                 new AniPayments()
                {
                    nombre = "Yape",
                    imagen = "http://www.carlossilvafit.com/wp-content/uploads/logo_yape.png",
                }
            };
            return lista2;
        }
    }
}

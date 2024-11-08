using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
namespace iadeApp
{
    public partial class MainPage : ContentPage
    {
        public class Kullanici
        {
            public int KullaniciNo { get; set; }
            public int SiparisSayisi { get; set; }
            public int IadeSayisi { get; set; }
            public double PuanOrtalamasi { get; set; }
        }
        private List<Kullanici> kullanicilar = new List<Kullanici>();

        public MainPage()
        {
            InitializeComponent();
        }
        // Kullanıcı Ekle Butonu Tıklama Olayı
        private void OnKullaniciEkleClicked(object sender, EventArgs e)
        {
            try
            {
                int kullaniciNo = int.Parse(KullaniciNoEntry.Text);
                int siparisSayisi = int.Parse(SiparisSayisiEntry.Text);
                int iadeSayisi = int.Parse(IadeSayisiEntry.Text);
                double puanOrtalamasi = double.Parse(PuanOrtalamasiEntry.Text);

                // Giriş verilerinin doğrulanması
                if (siparisSayisi < 0 || iadeSayisi < 0 || iadeSayisi > siparisSayisi || puanOrtalamasi < 1 || puanOrtalamasi > 5)
                {
                    DisplayAlert("Hata", "Geçersiz veri girişi. Lütfen kontrol edin.", "Tamam");
                    return;
                }

                // Yeni kullanıcıyı listeye ekle
                kullanicilar.Add(new Kullanici
                {
                    KullaniciNo = kullaniciNo,
                    SiparisSayisi = siparisSayisi,
                    IadeSayisi = iadeSayisi,
                    PuanOrtalamasi = puanOrtalamasi
                });

                DisplayAlert("Başarılı", "Kullanıcı başarıyla eklendi.", "Tamam");

                // Giriş alanlarını temizle
                KullaniciNoEntry.Text = "";
                SiparisSayisiEntry.Text = "";
                IadeSayisiEntry.Text = "";
                PuanOrtalamasiEntry.Text = "";
            }
            catch
            {
                DisplayAlert("Hata", "Lütfen tüm alanlara geçerli değerler girin.", "Tamam");
            }
        }

        // Genel İstatistikleri Göster Butonu Tıklama Olayı
        private void OnGenelIstatistikleriGosterClicked(object sender, EventArgs e)
        {
            if (kullanicilar.Count == 0)
            {
                DisplayAlert("Bilgi", "Genel istatistikleri göstermek için en az bir kullanıcı ekleyin.", "Tamam");
                return;
            }

            double ortalamaSatinAlma = kullanicilar.Average(k => k.SiparisSayisi - k.IadeSayisi);
            double iadeOrani = kullanicilar.Count(k => k.IadeSayisi > 0) / (double)kullanicilar.Count * 100;
            double hicBesVermeyenOrani = kullanicilar.Count(k => k.PuanOrtalamasi < 5) / (double)kullanicilar.Count * 100;
            double puanOrtalamasiDusukOrani = kullanicilar.Count(k => k.PuanOrtalamasi < 2) / (double)kullanicilar.Count * 100;

            var enCokUrunAlan = kullanicilar.OrderByDescending(k => k.SiparisSayisi).First();

            string sonuc = $"Kullanıcı Başına Ortalama Satın Alınan Ürün Sayısı: {ortalamaSatinAlma:F2}\n" +
                           $"En az bir ürün iade eden kullanıcı oranı: %{iadeOrani:F2}\n" +
                           $"Hiç 5 puan vermeyen kullanıcı oranı: %{hicBesVermeyenOrani:F2}\n" +
                           $"Puan ortalaması 2’den düşük olan kullanıcı oranı: %{puanOrtalamasiDusukOrani:F2}\n" +
                           $"En çok ürün alan kullanıcı no: {enCokUrunAlan.KullaniciNo}, " +
                           $"Satın Aldığı Ürün Sayısı: {enCokUrunAlan.SiparisSayisi - enCokUrunAlan.IadeSayisi}, " +
                           $"İade Ettiği Ürün Sayısı: {enCokUrunAlan.IadeSayisi}, " +
                           $"Verdiği Puan Ortalaması: {enCokUrunAlan.PuanOrtalamasi:F2}";

            DisplayAlert("Genel İstatistikler", sonuc, "Tamam");
        }

        // Yeni Eklenen: Sipariş Verenleri Göster Butonu Tıklama Olayı
        private void OnSiparisVerenleriGosterClicked(object sender, EventArgs e)
        {
            if (kullanicilar.Count == 0)
            {
                DisplayAlert("Bilgi", "Sipariş veren kullanıcı yok.", "Tamam");
                return;
            }

            string mesaj = "Sipariş Veren Kullanıcılar:\n\n";
            foreach (var kullanici in kullanicilar)
            {
                mesaj += $"Kullanıcı No: {kullanici.KullaniciNo}\n" +
                         $"Sipariş Verdiği Ürün Sayısı: {kullanici.SiparisSayisi}\n" +
                         $"İade Ettiği Ürün Sayısı: {kullanici.IadeSayisi}\n" +
                         $"Verilen Puan Ortalaması: {kullanici.PuanOrtalamasi:F2}\n\n";
            }

            DisplayAlert("Sipariş Veren Kullanıcılar", mesaj, "Tamam");
        }

    }

}

<h1>Happy Hour Game Studio</h1>
<h2>Archero Case</h2>
<p>Kısaca proje içeriklerinden bahsetmek istiyorum.</p>
<p>
  Oyun ayarları <code>Assets &gt; GameFolders &gt; Datas</code> klasöründe 
  Scriptable Objeler kullanılarak tutulmuştur. Gerekli ayarları buradan 
  düzenleyebilirsiniz.
</p>
<h3>Kullanılan Design Pattern’ler</h3>
<ul>
  <li><strong>Strategy Pattern</strong> - Atak üzerindeki yetenek(skill) etkilerini yönetmek için kullanıldı.</li>
  <li><strong>State Pattern</strong> - Koşma ve yürüme durumlarını yönetmek için kullanıldı.</li>
  <li><strong>Event Driven Pattern</strong> - Yetenek(skill), Durum(state) ve Duraklatma(pause) yönetimi için kullanıldı.</li>
  <li><strong>Object Pooling</strong> - Düşman ve Ok üretiminde performans iyileştirmesi için kullanıldı.</li>
  <li><strong>Singleton Pattern</strong> - Temel yöneticiler (manager) ve verilerin tekil örneği için kullanıldı.</li>
</ul>
<p>En yakın düşmanın bulunmasında <em><strong>Sorting Algoritması</strong></em> kullanıldı.</p>
<p>Ok atışında <em><strong>Projectile Motion</strong></em> kullanıldı.</p>

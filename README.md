# 🧱 Entity Framework Core - ModelBuilder Rehberi

## Giriş

Entity Framework Core’da `ModelBuilder`, uygulamadaki C# sınıflarını (entity'leri) veritabanı tablolarına dönüştürürken yapılandırma yapmamızı sağlayan bir nesnedir. Fluent API yaklaşımıyla konfigürasyonları merkezi bir noktada toplayarak, veritabanı şeması üzerinde tam kontrol sağlar.

Bu yapılandırmalar genellikle `DbContext` sınıfı içerisinde `OnModelCreating(ModelBuilder modelBuilder)` metodu override edilerek tanımlanır.

### Kullanım Örneği:

```csharp
public class AppDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id); // Primary Key

        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique(); // Unique index
    }
}
```
# 🔑 1. HasKey()
HasKey(), bir varlık sınıfı için birincil anahtarı (primary key) belirlemek için kullanılır. Genellikle, sınıfın Id veya Guid gibi bir özelliği otomatik olarak birincil anahtar olarak kabul edilir, ancak bunu manuel olarak da belirleyebilirsiniz.

``` csharp
modelBuilder.Entity<Post>()
    .HasKey(p => p.Id); // Id özelliğini birincil anahtar olarak belirler.

```
# 📈 2. HasIndex()
HasIndex(), belirli bir kolona (veya birden fazla kolona) indeks ekler. İndeksler, sorgu performansını artırmak için kullanılır. IsUnique() metodu ile indeksin benzersiz (unique) olması sağlanabilir.

``` csharp
modelBuilder.Entity<User>()
    .HasIndex(u => u.UserName)
    .IsUnique(); // UserName özelliğine bir benzersiz indeks ekler.

```
# 📏 3. Property()
Property(), bir varlığın özelliğini yapılandırmak için kullanılır. Bu metotla, özelliklerin veri tipi, uzunluğu, nullable olup olmadığı gibi özellikleri belirleyebilirsiniz.

``` csharp
modelBuilder.Entity<Post>()
    .Property(p => p.Title)
    .HasMaxLength(200); // Title property'sinin maksimum uzunluğunu 200 karakterle sınırlıyor.

modelBuilder.Entity<User>()
    .Property(u => u.Email)
    .IsRequired(); // Email özelliğinin zorunlu (nullable değil) olduğunu belirtir.

```

# 🤝 4. HasOne() ve WithMany()
HasOne() ve WithMany(), bir varlıkla başka bir varlık arasındaki ilişkiyi tanımlar. Bu ilişkiyi HasOne() metodu ile belirtirsiniz ve karşılık gelen WithMany() metodu ile diğer varlığı tanımlarsınız.
``` csharp
modelBuilder.Entity<Post>()
    .HasOne(p => p.User)  // Her Post bir User'a sahip
    .WithMany(u => u.Posts) // Her User birden fazla Post'a sahip olabilir
    .HasForeignKey(p => p.UserId); // UserId kolonu dış anahtar olarak kullanılır.

```

# 🧩 5. HasForeignKey()
HasForeignKey(), bir ilişkiyi tanımlarken, hangi özelliğin dış anahtar (foreign key) olarak kullanılacağını belirler.
``` csharp
modelBuilder.Entity<Post>()
    .HasOne(p => p.User)
    .WithMany(u => u.Posts)
    .HasForeignKey(p => p.UserId); // UserId, Post tablosundaki dış anahtar olarak belirtilmiştir.

```
#  🗂️ 6. ToTable()
ToTable(), bir varlık sınıfının hangi tabloya karşılık geldiğini belirtir. Bu genellikle varsayılan tablo ismini değiştirmek için kullanılır.
``` csharp
modelBuilder.Entity<Post>()
    .HasOne(p => p.User)
    .WithMany(u => u.Posts)
    .HasForeignKey(p => p.UserId); // UserId, Post tablosundaki dış anahtar olarak belirtilmiştir.

```
# 👓 7. ToView()
ToView(), bir varlık sınıfının veritabanındaki bir görünüme (view) karşılık gelmesini sağlar.
``` csharp
modelBuilder.Entity<Post>()
    .ToView("PostView"); // Post varlığı, PostView görünümüne karşılık gelir.

```
#🚫 8. Ignore()
Ignore(), bir varlığın belirli bir özelliğini EF Core tarafından dikkate alınmaması için kullanılır. Bu özellik veritabanına yansıtılmaz.
``` csharp
modelBuilder.Entity<User>()
    .Ignore(u => u.Password); // Password özelliği veritabanında dikkate alınmaz.


```
# 🔁 9. HasMany() ve WithOne()
HasMany() ve WithOne(), birden fazla varlık arasında bir ilişkiyi belirtir. Örneğin, bir User'ın birden fazla Post'u olabilir.
``` csharp
modelBuilder.Entity<User>()
    .HasMany(u => u.Posts) // Her User, birden fazla Post'a sahip olabilir.
    .WithOne(p => p.User) // Her Post, bir User'a sahip olmalıdır.
    .HasForeignKey(p => p.UserId); // Post'taki UserId, dış anahtar olarak kullanılır.


```
# 🔄 10. HasConversion()
HasConversion(), veritabanı için belirli bir özelliğin veri türünü dönüştürmenizi sağlar. Örneğin, bir enum türünü veritabanında string olarak depolamak.
``` csharp
modelBuilder.Entity<User>()
    .Property(u => u.Status)
    .HasConversion(
        v => v.ToString(), // enum'ı string'e dönüştür
        v => (UserStatus)Enum.Parse(typeof(UserStatus), v)); // string'i enum'a dönüştür

```
# 🧬 11.  UseCollation()
UseCollation(), bir kolon için sıralama (collation) belirler. Bu, metin verilerinin sıralanma şeklini etkiler.
``` csharp
modelBuilder.Entity<User>()
    .Property(u => u.UserName)
    .UseCollation("Latin1_General_CI_AS"); // Kullanıcı adı kolonu için sıralama tanımlar.

```
# 🧪 12. ToTable()
ToTable(), bir varlık sınıfının hangi tabloya karşılık geldiğini belirtir. Bu genellikle varsayılan tablo ismini değiştirmek için kullanılır.
``` csharp
modelBuilder.Entity<Post>()
    .HasOne(p => p.User)
    .WithMany(u => u.Posts)
    .HasForeignKey(p => p.UserId); // UserId, Post tablosundaki dış anahtar olarak belirtilmiştir.

```
# ⏱️ 13. HasCheckConstraint()
HasCheckConstraint(), bir kolon için özel bir kontrol kısıtlaması ekler. Bu kısıtlama, veritabanı düzeyinde değerlerin geçerliliğini kontrol eder.
``` csharp
modelBuilder.Entity<Post>()
    .Property(p => p.Title)
    .HasCheckConstraint("CK_Post_Title_Length", "LEN(Title) > 3"); // Title uzunluğu 3'ten fazla olmalı.


```
# 14. IsConcurrencyToken()
IsConcurrencyToken(), bir özelliği eşzamanlılık (concurrency) denetimi için belirler. Bu, veritabanındaki verilerin aynı anda birden fazla işlem tarafından değiştirilmesini engeller.
``` csharp
modelBuilder.Entity<Post>()
    .Property(p => p.CreatedAt)
    .HasDefaultValueSql("GETDATE()"); // CreatedAt kolonu için varsayılan olarak geçerli tarih saat ekler.
    .IsConcurrencyToken(); // RowVersion, eşzamanlılık denetimi için kullanılır.

```
# 15. HasMany() ve HasOne()
HasMany() ve HasOne(), veritabanındaki ilişkilerde birden fazla varlıkla ilişki kurarken kullanılır.
``` csharp
modelBuilder.Entity<Order>()
    .HasMany(o => o.Items)
    .WithOne(i => i.Order)
    .HasForeignKey(i => i.OrderId);

```

# 🗑️ 16.OnDelete()
OnDelete(), Entity Framework Core'da bir ilişkide parent (ana) kayıt silindiğinde child (alt) kayıtların ne olacağını belirtmek için kullanılır.
``` csharp
modelBuilder.Entity<Comment>()
    .HasOne(c => c.Tweet)
    .WithMany(t => t.Comments)
    .HasForeignKey(c => c.TweetId)
    .OnDelete(DeleteBehavior.Cascade);

```
### 🔧 DeleteBehavior Seçenekleri

| Seçenek            | Açıklama                                                                 |
|--------------------|--------------------------------------------------------------------------|
| `Cascade`          | Parent silinirse, child kayıtlar **otomatik olarak silinir.**            |
| `Restrict`         | Parent silinemez, önce bağlı child kayıtlar **manuel olarak silinmelidir.** |
| `SetNull`          | Parent silinirse, child tablodaki foreign key sütunu **null yapılır.** FK nullable olmalıdır. |
| `NoAction`         | Silme işlemine EF müdahale etmez. **Veritabanı kurallarına bırakılır.** |
| `ClientSetNull`    | EF Core bellekte çalışırken FK alanını null yapar. Genelde kullanılmaz. |

Bu notlar, Entity Framework Core'da ModelBuilder ile konfigürasyon yaparken başvurman için hazırlanmıştır. Projelerinde ilişki tanımlamaları, performans iyileştirmeleri ve özelleştirme ihtiyaçlarında bu dökümandan faydalanabilirsin.
Hazırlayan: Mert Ağralı 👨‍💻




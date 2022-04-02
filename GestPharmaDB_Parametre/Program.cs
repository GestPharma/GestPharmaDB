using Microsoft.VisualBasic;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Transactions;

namespace GestPharmaDB_Parametre
    {
    class Program
        {
        // Paramètre de connexion à la BDD CIS
        const string CONNECTION_STRING = @"Data Source=LENOVO;Initial Catalog=CIS;Integrated Security=True";
        static readonly string BACKUP_STRING = @$"Backup database CIS to disk='C:\_BACKUP-CIS\CIS-{DateTime.UtcNow:yyyy-MM-dd HH-mm-ss}.bak'";

        // Les fichiers à lire sur le WEB
        static readonly string[] tables = {
                            "https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=CIS_bdpm.txt"
                            ,"https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=CIS_CIP_bdpm.txt"
                            ,"https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=CIS_COMPO_bdpm.txt"
                            ,"https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=CIS_HAS_SMR_bdpm.txt"
                            ,"https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=CIS_HAS_ASMR_bdpm.txt"
                            ,"https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=HAS_LiensPageCT_bdpm.txt"
                            ,"https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=CIS_GENER_bdpm.txt"
                            ,"https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=CIS_CPD_bdpm.txt"
                            ,"https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=CIS_InfoImportantes.txt"
                          };
        static void Main(string[] args)
            {
            Console.Write($"\n\n Début BACKUP CIS ==> {DateTime.Now} => ");
            using SqlConnection connection = new(CONNECTION_STRING);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = BACKUP_STRING;
            connection.Open();
            try { cmd.ExecuteNonQuery(); Console.Write("B"); }
            catch { Console.Write("x"); }
            connection.Close();
            Console.WriteLine($"\n\n fin BACKUP CIS ==> {DateTime.Now}");

            using WebClient client = new();
            for (int nbtable = 0; nbtable < tables.Length; nbtable++)
                {
                Stream stream = client.OpenRead(tables[nbtable]);
                using StreamReader reader = new(stream);
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Console.OutputEncoding = System.Text.Encoding.Default;

                string content = System.Uri.UnescapeDataString(reader.ReadToEnd());
                string[] lignes = content.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (lignes.Length <= 2) { lignes = content.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries); }

                // using SqlConnection connection = new(CONNECTION_STRING);
                // using SqlCommand cmd = connection.CreateCommand();
                connection.Open();
                cmd.CommandType = CommandType.Text;
                Console.Write($"\n\n {tables[nbtable]} ==> {DateTime.Now}");
                string[] NTable = tables[nbtable].Split(new[] { "=" }, StringSplitOptions.TrimEntries);
                string[] Ntable = NTable[1].Split(new[] { "." }, StringSplitOptions.TrimEntries);
                cmd.CommandText = @$"TRUNCATE TABLE [dbo].{Ntable[0]}; ";
                try { cmd.ExecuteNonQuery(); Console.Write("*"); }
                catch { Console.Write("t"); }
                using (TransactionScope transactionScope = new())
                    {
                    try
                        {
                        foreach (string ligne in lignes)
                            {
                            Regex.Replace(ligne, @"[^\w\.@-]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
                            if (tables[nbtable].Contains("CIS_bdpm.txt"))
                                {
                                string lg = ligne + "\t";
                                string[] champs = lg.Split(new[] { "\t" }, StringSplitOptions.TrimEntries);
                                cmd.CommandText = @$"INSERT INTO[dbo].{"CIS_bdpm"}" +
                                                $"(cis,denomination,forme,administration,status_amm" +
                                                $",type_amm,etat_com,date_amm,statut_bdm" +
                                                $",autorisation_euro,titulaire,surveillance,TRIAL463)" +
                                            "VALUES" +
                                                $"(@cis,@denomination,@forme,@administration,@status_amm" +
                                                $",@type_amm,@etat_com,@date_amm,@statut_bdm" +
                                                $",@autorisation_euro,@titulaire,@surveillance,@TRIAL463);";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("cis", int.TryParse(champs[00], out int result) ? int.Parse(champs[00]) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("denomination", champs[01] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("forme", champs[02] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("administration", champs[03] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("status_amm", champs[04] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("type_amm", champs[05] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("etat_com", champs[06] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("date_amm", DateTime.TryParse(champs[07], out DateTime result1) ? DateTime.Parse(champs[07]) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("statut_bdm", champs[08] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("autorisation_euro", (object)DBNull.Value);  // char.Parse((string)(champs[09].ToString() ?? (object)DBNull.Value)) ); ;
                                cmd.Parameters.AddWithValue("titulaire", champs[10] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("surveillance", champs[11] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("TRIAL463", champs[12] ?? (object)DBNull.Value);
                                try { cmd.ExecuteNonQuery(); Console.Write("-"); }
                                catch
                                    {
                                    Console.Write("i");
                                    cmd.CommandText = @$"UPDATE [dbo].{"CIS_bdpm"} SET " +
                                                        $" denomination = @denomination,forme = @forme,administration = @administration,status_amm = @status_amm" +
                                                        $",type_amm = @type_amm,etat_com = @etat_com,date_amm = @date_amm,statut_bdm = @statut_bdm" +
                                                        $",autorisation_euro = @autorisation_euro,titulaire = @titulaire,surveillance = @surveillance,TRIAL463 = @TRIAL463 " +
                                                        $" WHERE cis = @cis;";
                                    try { cmd.ExecuteNonQuery(); Console.Write("."); }
                                    catch { Console.Write("u"); }
                                    };
                                }
                            else if (tables[nbtable].Contains("CIS_CIP_bdpm.txt"))
                                {
                                string lg = ligne + "\t";
                                string[] champs = lg.Split(new[] { "\t" }, StringSplitOptions.TrimEntries);
                                cmd.CommandText = @$"INSERT INTO[dbo].{"CIS_CIP_bdpm"}" +
                                                $"(cis,cip7,libelle_prez,statut_administratif,etat_com" +
                                                $",date_com,cip13,agrement,taux_remboursement" +
                                                $",prix1,prix2,prix3,indications,TRIAL476)" +
                                            "VALUES" +
                                                $"(@cis,@cip7,@libelle_prez,@statut_administratif,@etat_com" +
                                                $",@date_com,@cip13,@agrement,@taux_remboursement" +
                                                $",@prix1,@prix2,@prix3,@indications,@TRIAL476);";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("cis", int.TryParse(champs[00], out int result) ? int.Parse(champs[00]) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("cip7", int.TryParse(champs[01], out int result1) ? int.Parse(champs[01]) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("libelle_prez", champs[02] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("statut_administratif", champs[03] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("etat_com", champs[04] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("date_com", DateTime.TryParse(champs[05], out DateTime result2) ? DateTime.Parse(champs[05]) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("cip13", long.TryParse(champs[06], out long result3) ? long.Parse(champs[06]) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("agrement", champs[07] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("taux_remboursement", champs[08] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("prix1", champs[09] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("prix2", champs[10] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("prix3", champs[11] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("indications", champs[12] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("TRIAL476", (object)DBNull.Value);
                                try { cmd.ExecuteNonQuery(); Console.Write("-"); }
                                catch
                                    {
                                    Console.Write("i");
                                    cmd.CommandText = @$"UPDATE [dbo].{"CIS_CIP_bdpm"} SET " +
                                                        $" cip7 = @cip7,libelle_prez = @libelle_prez,statut_administratif = @statut_administratif,etat_com = @etat_com" +
                                                        $",date_com = @date_com,cip13 = @cip13,agrement = @agrement,taux_remboursement = @taux_remboursement" +
                                                        $",prix1 = @prix1,prix2 = @prix2,prix3 = @prix3,indications = @indications,TRIAL476 = @TRIAL476 " +
                                                        $" WHERE cis = @cis;";
                                    try { cmd.ExecuteNonQuery(); Console.Write("."); }
                                    catch { Console.Write("u"); }
                                    };
                                }
                            else if (tables[nbtable].Contains("CIS_COMPO_bdpm.txt"))
                                {
                                string lg = ligne + "\t";
                                string[] champs = lg.Split(new[] { "\t" }, StringSplitOptions.TrimEntries);
                                cmd.CommandText = @$"INSERT INTO[dbo].{"CIS_COMPO_bdpm"}" +
                                                $"(cis,designation,code,denomination,dosage" +
                                                $",reference,nature,liaison,rien" +
                                                $",TRIAL492)" +
                                            "VALUES" +
                                                $"(@cis,@designation,@code,@denomination,@dosage" +
                                                $",@reference,@nature,@liaison,@rien" +
                                                $",@TRIAL492);";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("cis", int.TryParse(champs[00], out int result) ? int.Parse(champs[00]) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("designation", champs[01] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("code", champs[02] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("denomination", champs[03] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("dosage", champs[04] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("reference", champs[05] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("nature", champs[06] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("liaison", long.TryParse(champs[07], out long result1) ? long.Parse(champs[07]) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("rien", champs[08] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("TRIAL492", (object)DBNull.Value);
                                try { cmd.ExecuteNonQuery(); Console.Write("-"); }
                                catch
                                    {
                                    Console.Write("i");
                                    cmd.CommandText = @$"UPDATE [dbo].{"CIS_COMPO_bdpm"} SET " +
                                                        $" designation = @designation,code = @code,denomination = @denomination,dosage = @dosage" +
                                                        $",reference = @reference,reference = @reference,nature = @nature,liaison = @liaison" +
                                                        $",rien = @rien,TRIAL492 = @TRIAL492 " +
                                                        $" WHERE cis = @cis;";
                                    try { cmd.ExecuteNonQuery(); Console.Write("."); }
                                    catch { Console.Write("u"); }
                                    };
                                }
                            else if (tables[nbtable].Contains("CIS_CPD_bdpm.txt"))
                                {
                                string lg = ligne + "\t";
                                string[] champs = lg.Split(new[] { "\t" }, StringSplitOptions.TrimEntries);
                                cmd.CommandText = @$"INSERT INTO[dbo].{"CIS_CPD_bdpm"}" +
                                                $"(cis,conditions" +
                                                $",TRIAL509)" +
                                            "VALUES" +
                                                $"(@cis,@conditions" +
                                                $",@TRIAL509);";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("cis", int.TryParse(champs[00], out int result) ? int.Parse(champs[00]) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("conditions", champs[01] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("TRIAL509", (object)DBNull.Value);
                                try { cmd.ExecuteNonQuery(); Console.Write("-"); }
                                catch
                                    {
                                    Console.Write("i");
                                    cmd.CommandText = @$"UPDATE [dbo].{"CIS_CPD_bdpm"} SET " +
                                                        $" conditions = @conditions" +
                                                        $",TRIAL509 = @TRIAL509 " +
                                                        $" WHERE cis = @cis;";
                                    try { cmd.ExecuteNonQuery(); Console.Write("."); }
                                    catch { Console.Write("u"); }
                                    };
                                }
                            else if (tables[nbtable].Contains("CIS_GENER_bdpm.txt"))
                                {
                                string lg = ligne + "\t";
                                string[] champs = lg.Split(new[] { "\t" }, StringSplitOptions.TrimEntries);
                                cmd.CommandText = @$"INSERT INTO[dbo].{"CIS_GENER_bdpm"}" +
                                                $"(identifiant,libelle,cis,type_gen" +
                                                $",tri,rien" +
                                                $",TRIAL515)" +
                                            "VALUES" +
                                                $"(@identifiant,@libelle,@cis,@type_gen" +
                                                $",@tri,@rien" +
                                                $",@TRIAL515);";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("identifiant", champs[00] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("libelle", champs[01] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("cis", int.TryParse(champs[02], out int result1) ? int.Parse(champs[02]) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("type_gen", int.TryParse(champs[03], out int result2) ? int.Parse(champs[03]) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("tri", int.TryParse(champs[04], out int result3) ? int.Parse(champs[04]) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("rien", champs[05] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("TRIAL515", (object)DBNull.Value);
                                try { cmd.ExecuteNonQuery(); Console.Write("-"); }
                                catch
                                    {
                                    Console.Write("i");
                                    cmd.CommandText = @$"UPDATE [dbo].{"CIS_GENER_bdpm"} SET " +
                                                        $" identifiant = @identifiant,libelle = @libelle,cis = @cis,type_gen = @type_gen" +
                                                        $",tri = @tri" +
                                                        $",rien = @rien,TRIAL515 = @TRIAL515 " +
                                                        $" WHERE cis = @cis;";
                                    try { cmd.ExecuteNonQuery(); Console.Write("."); }
                                    catch { Console.Write("u"); }
                                    };
                                }
                            else if (tables[nbtable].Contains("CIS_HAS_ASMR_bdpm.txt"))
                                {
                                string lg = ligne + "\t";
                                string[] champs = lg.Split(new[] { "\t" }, StringSplitOptions.TrimEntries);
                                cmd.CommandText = @$"INSERT INTO[dbo].{"CIS_HAS_ASMR_bdpm"}" +
                                                $"(cis,code_has,motif,date_avis" +
                                                $",valeur_asmr,libelle_asmr" +
                                                $",TRIAL518)" +
                                            "VALUES" +
                                                $"(@cis,@code_has,@motif,@date_avis" +
                                                $",@valeur_asmr,@libelle_asmr" +
                                                $",@TRIAL518);";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("cis", int.TryParse(champs[00], out int result) ? int.Parse(champs[00]) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("code_has", champs[01] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("motif", champs[02] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("date_avis", DateTime.TryParse(champs[03], out DateTime result1) ? DateTime.Parse(champs[03]) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("valeur_asmr", champs[04] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("libelle_asmr", champs[05] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("TRIAL518", (object)DBNull.Value);
                                try { cmd.ExecuteNonQuery(); Console.Write("-"); }
                                catch
                                    {
                                    Console.Write("i");
                                    cmd.CommandText = @$"UPDATE [dbo].{"CIS_HAS_ASMR_bdpm"} SET " +
                                                        $" code_has = @code_has,motif = @motif,date_avis = @date_avis" +
                                                        $",valeur_asmr = @valeur_asmr" +
                                                        $",libelle_asmr = @libelle_asmr,TRIAL518 = @TRIAL518 " +
                                                        $" WHERE cis = @cis;";
                                    try { cmd.ExecuteNonQuery(); Console.Write("."); }
                                    catch { Console.Write("u"); }
                                    };
                                }
                            else if (tables[nbtable].Contains("CIS_HAS_SMR_bdpm.txt"))
                                {
                                string lg = ligne + "\t";
                                string[] champs = lg.Split(new[] { "\t" }, StringSplitOptions.TrimEntries);
                                cmd.CommandText = @$"INSERT INTO[dbo].{"CIS_HAS_SMR_bdpm"}" +
                                                $"(cis,code_has,motif,date_avis" +
                                                $",valeur_smr,libelle_smr" +
                                                $",TRIAL525)" +
                                            "VALUES" +
                                                $"(@cis,@code_has,@motif,@date_avis" +
                                                $",@valeur_smr,@libelle_smr" +
                                                $",@TRIAL525);";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("cis", int.TryParse(champs[00], out int result) ? int.Parse(champs[00]) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("code_has", champs[01] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("motif", champs[02] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("date_avis", DateTime.TryParse(champs[03], out DateTime result1) ? DateTime.Parse(champs[03]) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("valeur_smr", champs[04] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("libelle_smr", champs[05] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("TRIAL525", (object)DBNull.Value);
                                try { cmd.ExecuteNonQuery(); Console.Write("-"); }
                                catch
                                    {
                                    Console.Write("i");
                                    cmd.CommandText = @$"UPDATE [dbo].{"CIS_HAS_SMR_bdpm"} SET " +
                                                        $" code_has = @code_has,motif = @motif,date_avis = @date_avis" +
                                                        $",valeur_smr = @valeur_smr" +
                                                        $",libelle_smr = @libelle_smr,TRIAL525 = @TRIAL525 " +
                                                        $" WHERE cis = @cis;";
                                    try { cmd.ExecuteNonQuery(); Console.Write("."); }
                                    catch { Console.Write("u"); }
                                    };
                                }
                            else if (tables[nbtable].Contains("CIS_InfoImportantes.txt"))
                                {
                                string lg = ligne + "\t";
                                string[] champs = lg.Split(new[] { "\t" }, StringSplitOptions.TrimEntries);
                                cmd.CommandText = @$"INSERT INTO[dbo].{"CIS_InfoImportantes"}" +
                                                $"(cis,date_debut,date_fin" +
                                                $",texte" +
                                                $",TRIAL535)" +
                                            "VALUES" +
                                                $"(@cis,@date_debut,@date_fin" +
                                                $",@texte" +
                                                $",@TRIAL535);";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("cis", int.TryParse(champs[00], out int result) ? int.Parse(champs[00]) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("date_debut", DateTime.TryParse(champs[01], out DateTime result1) ? DateTime.Parse(champs[01]) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("date_fin", DateTime.TryParse(champs[02], out DateTime result2) ? DateTime.Parse(champs[02]) : (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("texte", champs[03] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("TRIAL535", (object)DBNull.Value);
                                try { cmd.ExecuteNonQuery(); Console.Write("-"); }
                                catch
                                    {
                                    Console.Write("i");
                                    cmd.CommandText = @$"UPDATE [dbo].{"CIS_InfoImportantes"} SET " +
                                                        $" date_debut = @date_debut,date_fin = @date_fin" +
                                                        $",texte = @texte" +
                                                        $",TRIAL535 = @TRIAL535 " +
                                                        $" WHERE cis = @cis;";
                                    try { cmd.ExecuteNonQuery(); Console.Write("."); }
                                    catch { Console.Write("u"); }
                                    };
                                }
                            else if (tables[nbtable].Contains("HAS_LiensPageCT_bdpm.txt"))
                                {
                                string lg = ligne + "\t";
                                string[] champs = lg.Split(new[] { "\t" }, StringSplitOptions.TrimEntries);
                                cmd.CommandText = @$"INSERT INTO[dbo].{"HAS_LiensPageCT_bdpm"}" +
                                                $"(code_has,lien_vers_avis_ct" +
                                                $",TRIAL541)" +
                                            "VALUES" +
                                                $"(@code_has,@lien_vers_avis_ct" +
                                                $",@TRIAL541);";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("code_has", champs[00] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("lien_vers_avis_ct", champs[01] ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("TRIAL541", (object)DBNull.Value);
                                try { cmd.ExecuteNonQuery(); Console.Write("-"); }
                                catch
                                    {
                                    Console.Write("i");
                                    cmd.CommandText = @$"UPDATE [dbo].{"HAS_LiensPageCT_bdpm"} SET " +
                                                        $" lien_vers_avis_ct = @[lien_vers_avis_ct]" +
                                                        $",TRIAL541 = @TRIAL541 " +
                                                        $" WHERE code_has = @code_has;";
                                    try { cmd.ExecuteNonQuery(); Console.Write("."); }
                                    catch { Console.Write("u"); }
                                    };
                                }
                            else { break; }
                            }
                        }
                    catch (TransactionException ex)
                        {
                        transactionScope.Dispose();
                        Console.WriteLine($"\n\n Abandon CIS {Ntable[0]} {ex.Message} ==> {DateTime.Now}");
                        break;
                        }
                    }
                Console.WriteLine($"\n\n Fin CIS ==> {DateTime.Now}");
            }
        }
    }
}

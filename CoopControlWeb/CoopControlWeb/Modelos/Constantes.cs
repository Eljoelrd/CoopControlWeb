namespace CoopControlWeb.Modelos
{
    public class Constantes
    {
        public List<string> SEXO { get; set; }
        public List<string> TIPOS_SANGRE { get; set; }
        public List<string> ESTADO_CIVIL { get; set; }



        public Constantes()

        {

            //Sexo
            SEXO = new List<string>();
            SEXO.Add("Masculino");
            SEXO.Add("Femenino");

            // Tipos de Sangre
            TIPOS_SANGRE = new List<string>();
            TIPOS_SANGRE.Add("O+");
            TIPOS_SANGRE.Add("O-");
            TIPOS_SANGRE.Add("A+");
            TIPOS_SANGRE.Add("A-");
            TIPOS_SANGRE.Add("B+");
            TIPOS_SANGRE.Add("B-");
            TIPOS_SANGRE.Add("AB+");
            TIPOS_SANGRE.Add("AB-");

            //Estado Civil
            ESTADO_CIVIL = new List<string>();
            ESTADO_CIVIL.Add("Casado");
            ESTADO_CIVIL.Add("Soltero");
            ESTADO_CIVIL.Add("Viudo");
            ESTADO_CIVIL.Add("Divorciado");
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace MVCWebApi.Models
{
    public class MetodosCuenta
    {
        //Alta cuenta 

        public void AltaCuenta(Cuenta Cuenta)
        {
            string connection = ConfigurationManager.ConnectionStrings["BDLocal"].ToString();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();

                SqlCommand comm = conn.CreateCommand();
                comm.CommandText = "CREAR CUENTA";
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add(new SqlParameter("@IdCuenta", Cuenta.Id1));
                comm.Parameters.Add(new SqlParameter("@TipoCuenta", Cuenta.Tipo_Cuenta1));
                comm.Parameters.Add(new SqlParameter("@Monto", Cuenta.Monto));
      
                comm.ExecuteNonQuery();

            }
        }

        //Baja Cuenta

        public void EliminarCuenta(int Id1)
        {
            string StrConn = ConfigurationManager.ConnectionStrings["BDLocal"].ToString();

            using (SqlConnection conn = new SqlConnection(StrConn))
            {
                conn.Open();

                SqlCommand comm = new SqlCommand("BAJA-CUENTA", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Add(new SqlParameter("@IdCuenta", Id1));

                comm.ExecuteNonQuery();
            }

        }

        //Obtener saldo 

        public decimal ObtenerSaldo(int idCuenta)
        {

            string StrConn = ConfigurationManager.ConnectionStrings["BDLocal"].ToString();
            decimal saldo = 0;

            using (SqlConnection connec = new SqlConnection(strConn))
            {
                connec.Open();

                SqlCommand comm = new SqlCommand("MOSTRAR SALDO", connec);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Add(new SqlParameter("@idCuenta", Id1));

                SqlDataReader reader = comm.ExecuteReader();

                if (reader.Read())
                {
                    saldo = reader.GetDecimal(4);
                }

            }
            return saldo;
        }

        //mostrar cuenta y ultimos movimientos 
        public Cuenta ObtenerCuenta(int Id1)
        {
            Cuenta Cuenta = null;
            List<Movimientos> lista = new List<Movimientos>();



            string StrConn = ConfigurationManager.ConnectionStrings["BDLocal"].ToString();

            using (SqlConnection conn = new SqlConnection(StrConn))
            {
                conn.Open();

                SqlCommand comm = conn.CreateCommand();
                comm.CommandText = "MOSTRAR CUENTA";
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add(new SqlParameter("@idCuenta", Id1));

                SqlDataReader dr = comm.ExecuteReader();

                if (dr.Read())
                {
                    int Id1 = dr.GetInt32(1); 
                    int IdCliente1 = dr.GetInt32(2);
                    string Tipo_Cuenta1 = dr.GetStrinf(3).Trim();
                    double Monto = dr.GetDouble(4);

                    Cuenta = new Cuenta(Id1,IdCliente1,Tipo_Cuenta1,Monto);

                    comm = conn.CreateCommand();
                    comm.CommandText = "LISTAR MOVIMIENTOS";
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.Add(new SqlParameter("@idCuenta", Id1));
                    dr.Close();
                    dr = comm.ExecuteReader();

                    while (dr.Read())
                    {
                        DateTime fechahora = dr.GetDateTime(2);
                        double monto = dr.GetDouble(3);
                        int idCuenta = dr.GetInt32(4);
                        string tipoCuenta = dr.GetString(5).Trim();
                        string tipoMovimiento = dr.GetString(6).Trim();

                        movimiento = new Movimiento(fechahora, monto, idCuenta, tipoCuenta, tipoMovimiento);
                        Cuenta.Movimientos.Add(movimiento);
                       

                    }
                    dr.Close();

                }

            }

            return Cuenta;
        }
    }
}
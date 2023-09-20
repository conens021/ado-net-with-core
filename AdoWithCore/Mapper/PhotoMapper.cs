using AdoWithCore.Entities;
using System.Data;
using System.Data.SqlClient;

namespace AdoWithCore.Mapper
{
    public class PhotoMapper
    {
        public SqlDataReader DataReader { get; set; } = default !;
        public DataTable PhotoTable { get; set; } = default!;
        public List<Photo> ToList()
        {
            List<Photo> photos  = new List<Photo>();

            if (DataReader.HasRows)
            {
                while (DataReader.Read())
                {
                    photos.Add(new Photo() { 
                        Id = Convert.ToInt32(DataReader["Id"]), 
                        Path = Convert.ToString(DataReader["Path"]),
                        CreatedAt = Convert.ToDateTime(DataReader["CreatedAt"]),
                        UpdatedAt = Convert.ToDateTime(DataReader["UpdatedAt"]),
                        GalleryId = Convert.ToInt32(DataReader["GalleryId"]) 
                    });
                }

            }
            return photos;
        }


        public static List<Photo> fromDataSetToList(DataTable photoTable) {

            List<Photo> photos = new List<Photo>();

            foreach (DataRow dr in photoTable.Rows)
            {
                photos.Add(new Photo()
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Path = Convert.ToString(dr["Path"]),
                    CreatedAt = Convert.ToDateTime(dr["CreatedAt"]),
                    UpdatedAt = Convert.ToDateTime(dr["UpdatedAt"]),
                    GalleryId = Convert.ToInt32(dr["GalleryId"])
                });
            }

            return photos;

        }


    }
}

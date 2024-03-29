using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.D3.Menger
{
    public class MengerSponge : MonoBehaviour
    {
        [SerializeField] Transform[] _vertices;
        [SerializeField] private SpongeProperties _properties;

        void Start()
        {
            var cube = new Cube(Utils.GetPositions(_vertices), 0, _properties);
        }
    }

    public class Cube
    {
        private readonly Vector3[] _vertices;
        private readonly int _currentIterator;
        private readonly SpongeProperties _properties;
        private GameObject Figure;

        public Cube(Vector3[] vertices, int currentIterator, SpongeProperties properties)
        {
            _vertices = vertices;
            _currentIterator = currentIterator;
            _properties = properties;
            Main.Instance.StartCoroutine(GenerateChildren());
        }


        IEnumerator GenerateChildren()
        {
            yield return new WaitForSeconds(_properties.Delay);
            var children = GenerateInsideCubes();
            foreach (var child in children)
            {
                var obj = SpawnObject(child._vertices[0]);
                child.Figure = obj;
            }
            Object.Destroy(Figure);
        }

        private GameObject SpawnObject(Vector3 pivotPos)
        {
            var res = Object.Instantiate(_properties.Prefab, _properties.Parent);
            // TODO position, rotation fixes
            return res;
        }

        public List<Cube> GenerateInsideCubes()
        {
            var vertices = new Dictionary<string, Vector3>() {
                { "A", _vertices[0] },
                { "B", _vertices[1] },
                { "C", _vertices[2] },
                { "D", _vertices[3] },
                { "E", _vertices[4] },
                { "F", _vertices[5] },
                { "G", _vertices[6] },
                { "H", _vertices[7] },

                { "AB", Vector3.Lerp(_vertices[0], _vertices[1], 1/3f) },
                { "AD", Vector3.Lerp(_vertices[0], _vertices[3], 1/3f) },
                { "AE", Vector3.Lerp(_vertices[0], _vertices[4], 1/3f) },

                { "BA", Vector3.Lerp(_vertices[1], _vertices[0], 1/3f) },
                { "BC", Vector3.Lerp(_vertices[1], _vertices[2], 1/3f) },
                { "BF", Vector3.Lerp(_vertices[1], _vertices[5], 1/3f) },

                { "CB", Vector3.Lerp(_vertices[2], _vertices[1], 1/3f) },
                { "CD", Vector3.Lerp(_vertices[2], _vertices[3], 1/3f) },
                { "CG", Vector3.Lerp(_vertices[2], _vertices[6], 1/3f) },

                { "DA", Vector3.Lerp(_vertices[3], _vertices[0], 1/3f) },
                { "DC", Vector3.Lerp(_vertices[3], _vertices[2], 1/3f) },
                { "DH", Vector3.Lerp(_vertices[3], _vertices[7], 1/3f) },

                { "EA", Vector3.Lerp(_vertices[4], _vertices[0], 1/3f) },
                { "EH", Vector3.Lerp(_vertices[4], _vertices[7], 1/3f) },
                { "EF", Vector3.Lerp(_vertices[4], _vertices[5], 1/3f) },

                { "FE", Vector3.Lerp(_vertices[5], _vertices[4], 1/3f) },
                { "FG", Vector3.Lerp(_vertices[5], _vertices[6], 1/3f) },
                { "FB", Vector3.Lerp(_vertices[5], _vertices[1], 1/3f) },

                { "GF", Vector3.Lerp(_vertices[6], _vertices[5], 1/3f) },
                { "GH", Vector3.Lerp(_vertices[6], _vertices[7], 1/3f) },
                { "GC", Vector3.Lerp(_vertices[6], _vertices[2], 1/3f) },

                { "HE", Vector3.Lerp(_vertices[7], _vertices[4], 1/3f) },
                { "HG", Vector3.Lerp(_vertices[7], _vertices[6], 1/3f) },
                { "HD", Vector3.Lerp(_vertices[7], _vertices[3], 1/3f) },
            };

            vertices.Add("ABD", GetOppositeVertice(vertices["A"], vertices["AB"], vertices["AD"]));
            vertices.Add("ABE", GetOppositeVertice(vertices["A"], vertices["AB"], vertices["AE"]));
            vertices.Add("ADE", GetOppositeVertice(vertices["A"], vertices["AD"], vertices["AE"]));

            vertices.Add("BAC", GetOppositeVertice(vertices["B"], vertices["BA"], vertices["BC"]));
            vertices.Add("BAF", GetOppositeVertice(vertices["B"], vertices["BA"], vertices["BF"]));
            vertices.Add("BCF", GetOppositeVertice(vertices["B"], vertices["BC"], vertices["BF"]));

            vertices.Add("CBD", GetOppositeVertice(vertices["C"], vertices["CB"], vertices["CD"]));
            vertices.Add("CBG", GetOppositeVertice(vertices["C"], vertices["CB"], vertices["CG"]));
            vertices.Add("CDG", GetOppositeVertice(vertices["C"], vertices["CD"], vertices["CG"]));

            vertices.Add("DAC", GetOppositeVertice(vertices["D"], vertices["DA"], vertices["DC"]));
            vertices.Add("DCH", GetOppositeVertice(vertices["D"], vertices["DC"], vertices["DH"]));
            vertices.Add("DAH", GetOppositeVertice(vertices["D"], vertices["DA"], vertices["DH"]));

            vertices.Add("EFH", GetOppositeVertice(vertices["E"], vertices["EF"], vertices["EH"]));
            vertices.Add("EAF", GetOppositeVertice(vertices["E"], vertices["EA"], vertices["EF"]));
            vertices.Add("EAH", GetOppositeVertice(vertices["E"], vertices["EA"], vertices["EH"]));

            vertices.Add("FEG", GetOppositeVertice(vertices["F"], vertices["FE"], vertices["FG"]));
            vertices.Add("FBE", GetOppositeVertice(vertices["F"], vertices["FB"], vertices["FE"]));
            vertices.Add("FBG", GetOppositeVertice(vertices["F"], vertices["FB"], vertices["FG"]));

            vertices.Add("GFH", GetOppositeVertice(vertices["G"], vertices["GF"], vertices["GH"]));
            vertices.Add("GCF", GetOppositeVertice(vertices["G"], vertices["GC"], vertices["GF"]));
            vertices.Add("GCH", GetOppositeVertice(vertices["G"], vertices["GC"], vertices["GH"]));
            
            vertices.Add("HEG", GetOppositeVertice(vertices["H"], vertices["HE"], vertices["HG"]));
            vertices.Add("HDE", GetOppositeVertice(vertices["H"], vertices["HD"], vertices["HE"]));
            vertices.Add("HDG", GetOppositeVertice(vertices["H"], vertices["HD"], vertices["HG"]));


            vertices.Add("ABDE", Vector3.Lerp(vertices["ABE"], vertices["DCH"], 1 / 3f));
            vertices.Add("BACF", Vector3.Lerp(vertices["BAF"], vertices["CDG"], 1 / 3f));
            vertices.Add("CBDG", Vector3.Lerp(vertices["CDG"], vertices["BAF"], 1 / 3f));
            vertices.Add("DACH", Vector3.Lerp(vertices["DCH"], vertices["ABE"], 1 / 3f));

            vertices.Add("EAFH", Vector3.Lerp(vertices["EAF"], vertices["HDG"], 1 / 3f));
            vertices.Add("FBEG", Vector3.Lerp(vertices["FBE"], vertices["GCH"], 1 / 3f));
            vertices.Add("GCFH", Vector3.Lerp(vertices["GCH"], vertices["FBE"], 1 / 3f));
            vertices.Add("HDEG", Vector3.Lerp(vertices["HDG"], vertices["EAF"], 1 / 3f));

            var nextIterator = _currentIterator + 1;

            var newCubes = new List<Cube>() { 
                new Cube(new Vector3[]{
                    vertices["A"],
                    vertices["AB"],
                    vertices["ABD"],
                    vertices["AD"],
                    vertices["AE"],
                    vertices["ABE"],
                    vertices["ABDE"],
                    vertices["ADE"],
                }, nextIterator, _properties),
                new Cube(new Vector3[]{
                    vertices["AB"],
                    vertices["BA"],
                    vertices["BAC"],
                    vertices["ABD"],
                    vertices["ABE"],
                    vertices["BAF"],
                    vertices["BACF"],
                    vertices["ABDE"],
                }, nextIterator, _properties),
                new Cube(new Vector3[]{
                    vertices["BA"],
                    vertices["B"],
                    vertices["BC"],
                    vertices["BAC"],
                    vertices["BAF"],
                    vertices["BF"],
                    vertices["BCF"],
                    vertices["BACF"],
                }, nextIterator, _properties),
                new Cube(new Vector3[]{
                    vertices["BC"],
                    vertices["CB"],
                    vertices["CBD"],
                    vertices["BAC"],
                    vertices["BCF"],
                    vertices["CBG"],
                    vertices["CBDG"],
                    vertices["BACF"],
                }, nextIterator, _properties),
                new Cube(new Vector3[]{
                    vertices["C"],
                    vertices["CD"],
                    vertices["CBD"],
                    vertices["CB"],
                    vertices["CG"],
                    vertices["CGH"],
                    vertices["CBDG"],
                    vertices["CBG"],
                }, nextIterator, _properties),
                new Cube(new Vector3[]{
                    vertices["CD"],
                    vertices["DC"],
                    vertices["DAC"],
                    vertices["CBD"],
                    vertices["CDG"],
                    vertices["DCH"],
                    vertices["DACH"],
                    vertices["CBDG"],
                }, nextIterator, _properties),
                new Cube(new Vector3[]{
                    vertices["D"],
                    vertices["DA"],
                    vertices["DAC"],
                    vertices["DC"],
                    vertices["DH"],
                    vertices["DAH"],
                    vertices["DACH"],
                    vertices["DCH"],
                }, nextIterator, _properties),
                new Cube(new Vector3[]{
                    vertices["DA"],
                    vertices["AD"],
                    vertices["ABD"],
                    vertices["DAC"],
                    vertices["DAH"],
                    vertices["ADE"],
                    vertices["ABDE"],
                    vertices["DACH"],
                }, nextIterator, _properties),
                new Cube(new Vector3[]{
                    vertices["AE"],
                    vertices["ABE"],
                    vertices["ABDE"],
                    vertices["ADE"],
                    vertices["EA"],
                    vertices["EAB"],
                    vertices["EAFH"],
                    vertices["AED"],
                }, nextIterator, _properties),
                new Cube(new Vector3[]{
                    vertices["BF"],
                    vertices["BCF"],
                    vertices["BACF"],
                    vertices["BAF"],
                    vertices["FB"],
                    vertices["FBG"],
                    vertices["FBEG"],
                    vertices["FEB"],
                }, nextIterator, _properties),
                new Cube(new Vector3[]{
                    vertices["CG"],
                    vertices["CDG"],
                    vertices["CBDG"],
                    vertices["CBG"],
                    vertices["GC"],
                    vertices["GCH"],
                    vertices["GCFH"],
                    vertices["G"],
                }, nextIterator, _properties),
            };
            return newCubes;

            // TODO "CHG" child not found problem
        }

        private static Vector3 GetOppositeVertice(Vector3 A, Vector3 B, Vector3 D)
        {
            Vector3 midpointBD = (B + D) / 2f;
            Vector3 vectorABD = midpointBD - A;
            Vector3 positionC = A + vectorABD * 2;
            return positionC;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.D3.Menger
{
    public class MegnerSpongeWithPrefabs : Singleton<MegnerSpongeWithPrefabs>
    {
        [SerializeField] Transform[] _vertices;
        [SerializeField] private SpongePropertiesWithPrefab _properties;

        void Start()
        {
            new CubeWithPrefab(Utils.GetPositions(_vertices), 0, _properties);
        }
    }

    public class CubeWithPrefab
    {
        private readonly Vector3[] _vertices;
        private readonly int _currentIterator;
        private readonly SpongePropertiesWithPrefab _properties;
        private readonly string _name;
        private GameObject Figure;

        public CubeWithPrefab(Vector3[] vertices, int currentIterator, SpongePropertiesWithPrefab properties, string name = "")
        {
            _vertices = vertices;
            _currentIterator = currentIterator;
            _properties = properties;
            _name = name;

            if (currentIterator < properties.IteratorLimit)
                MegnerSpongeWithPrefabs.Instance.StartCoroutine(GenerateChildren());
        }


        IEnumerator GenerateChildren()
        {
            yield return new WaitForSeconds(_properties.Delay);
            var children = GenerateInsideCubes();
            foreach (var child in children)
            {
                var obj = SpawnObject(child._vertices[0], child._name);
                child.Figure = obj;
            }
            Object.Destroy(Figure);
        }

        private GameObject SpawnObject(Vector3 pivotPos, string name)
        {
            var res = Object.Instantiate(_properties.Prefab, _properties.Parent);
            res.name = name;
            res.transform.localPosition = pivotPos;
            var targetScale = Vector3.one * (Figure != null ? Figure.transform.localScale.x / 3f : 1 / 3f);
            var startingScale = Vector3.one * (Figure != null ? Figure.transform.localScale.x : 1 / 3f);
            if (CanHaveAnimation(name))
            {
                res.transform.localScale = startingScale;
                MegnerSpongeWithPrefabs.Instance.StartCoroutine(Utils.DecreaseScaleAsync(res.transform, targetScale, _properties.Delay * 0.8f));
            }
            else
                res.transform.localScale = targetScale;
            return res;
        }

        public List<CubeWithPrefab> GenerateInsideCubes()
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

            var newCubes = new List<CubeWithPrefab>() { 
                // 1ST FLOOR
                new(new Vector3[]{
                    vertices["A"],
                    vertices["AB"],
                    vertices["ABD"],
                    vertices["AD"],
                    vertices["AE"],
                    vertices["ABE"],
                    vertices["ABDE"],
                    vertices["ADE"],
                }, nextIterator, _properties, "A"),
                new(new Vector3[]{
                    vertices["AB"],
                    vertices["BA"],
                    vertices["BAC"],
                    vertices["ABD"],
                    vertices["ABE"],
                    vertices["BAF"],
                    vertices["BACF"],
                    vertices["ABDE"],
                }, nextIterator, _properties, "AB"),
                new(new Vector3[]{
                    vertices["BA"],
                    vertices["B"],
                    vertices["BC"],
                    vertices["BAC"],
                    vertices["BAF"],
                    vertices["BF"],
                    vertices["BCF"],
                    vertices["BACF"],
                }, nextIterator, _properties, "BA"),
                new(new Vector3[]{
                    vertices["BAC"],
                    vertices["BC"],
                    vertices["CB"],
                    vertices["CBD"],
                    vertices["BACF"],
                    vertices["BCF"],
                    vertices["CBG"],
                    vertices["CBDG"],
                }, nextIterator, _properties, "BAC"),
                new(new Vector3[]{
                    vertices["CBD"],
                    vertices["CB"],
                    vertices["C"],
                    vertices["CD"],
                    vertices["CBDG"],
                    vertices["CBG"],
                    vertices["CG"],
                    vertices["CDG"],
                }, nextIterator, _properties, "CBD"),
                new(new Vector3[]{
                    vertices["DAC"],
                    vertices["CBD"],
                    vertices["CD"],
                    vertices["DC"],
                    vertices["DACH"],
                    vertices["CBDG"],
                    vertices["CDG"],
                    vertices["DCH"],
                }, nextIterator, _properties, "DAC"),
                new(new Vector3[]{
                    vertices["DA"],
                    vertices["DAC"],
                    vertices["DC"],
                    vertices["D"],
                    vertices["DAH"],
                    vertices["DACH"],
                    vertices["DCH"],
                    vertices["DH"],
                }, nextIterator, _properties, "DA"),
                new(new Vector3[]{
                    vertices["AD"],
                    vertices["ABD"],
                    vertices["DAC"],
                    vertices["DA"],
                    vertices["ADE"],
                    vertices["ABDE"],
                    vertices["DACH"],
                    vertices["DAH"],
                }, nextIterator, _properties, "DA"),

                // 2ND FLOOR
                new(new Vector3[]{
                    vertices["AE"],
                    vertices["ABE"],
                    vertices["ABDE"],
                    vertices["ADE"],
                    vertices["EA"],
                    vertices["EAF"],
                    vertices["EAFH"],
                    vertices["EAH"],
                }, nextIterator, _properties, "AE"),
                new(new Vector3[]{
                    vertices["BAF"],
                    vertices["BF"],
                    vertices["BCF"],
                    vertices["BACF"],
                    vertices["FBE"],
                    vertices["FB"],
                    vertices["FBG"],
                    vertices["FBEG"],
                }, nextIterator, _properties, "BAF"),
                new(new Vector3[]{
                    vertices["CBDG"],
                    vertices["CBG"],
                    vertices["CG"],
                    vertices["CDG"],
                    vertices["GCFH"],
                    vertices["GCF"],
                    vertices["GC"],
                    vertices["GCH"],
                }, nextIterator, _properties, "CG"),
                new(new Vector3[]{
                    vertices["DAH"],
                    vertices["DACH"],
                    vertices["DCH"],
                    vertices["DH"],
                    vertices["HDE"],
                    vertices["HDEG"],
                    vertices["HDG"],
                    vertices["HD"],
                }, nextIterator, _properties, "DAH"),

                // 3RD FLOOR
                new(new Vector3[]{
                    vertices["EA"],
                    vertices["EAF"],
                    vertices["EAFH"],
                    vertices["EAH"],
                    vertices["E"],
                    vertices["EF"],
                    vertices["EFH"],
                    vertices["EH"],
                }, nextIterator, _properties, "EA"),
                new(new Vector3[]{
                    vertices["EAF"],
                    vertices["FBE"],
                    vertices["FBEG"],
                    vertices["EAFH"],
                    vertices["EF"],
                    vertices["FE"],
                    vertices["FEG"],
                    vertices["EFH"],
                }, nextIterator, _properties, "EAF"),
                new(new Vector3[]{
                    vertices["FBE"],
                    vertices["FB"],
                    vertices["FBG"],
                    vertices["FBEG"],
                    vertices["FE"],
                    vertices["F"],
                    vertices["FG"],
                    vertices["FEG"],
                }, nextIterator, _properties, "FBE"),
                new(new Vector3[]{
                    vertices["FBEG"],
                    vertices["FBG"],
                    vertices["GCF"],
                    vertices["GCFH"],
                    vertices["FEG"],
                    vertices["FG"],
                    vertices["GF"],
                    vertices["GFH"],
                }, nextIterator, _properties, "FBEG"),
                new(new Vector3[]{
                    vertices["GCFH"],
                    vertices["GCF"],
                    vertices["GC"],
                    vertices["GCH"],
                    vertices["GFH"],
                    vertices["GF"],
                    vertices["G"],
                    vertices["GH"],
                }, nextIterator, _properties, "GCFH"),
                new(new Vector3[]{
                    vertices["HDEG"],
                    vertices["GCFH"],
                    vertices["GCH"],
                    vertices["HDG"],
                    vertices["HEG"],
                    vertices["GFH"],
                    vertices["GH"],
                    vertices["HG"],
                }, nextIterator, _properties, "HDEG"),
                new(new Vector3[]{
                    vertices["HDE"],
                    vertices["HDEG"],
                    vertices["HDG"],
                    vertices["HD"],
                    vertices["HE"],
                    vertices["HEG"],
                    vertices["HG"],
                    vertices["H"],
                }, nextIterator, _properties, "HDE"),
                new(new Vector3[]{
                    vertices["EAH"],
                    vertices["EAFH"],
                    vertices["HDEG"],
                    vertices["HDE"],
                    vertices["EH"],
                    vertices["EFH"],
                    vertices["HEG"],
                    vertices["HE"],
                }, nextIterator, _properties, "EAH"),

            };
            return newCubes;
        }

        private static Vector3 GetOppositeVertice(Vector3 A, Vector3 B, Vector3 D)
        {
            Vector3 midpointBD = (B + D) / 2f;
            Vector3 vectorABD = midpointBD - A;
            Vector3 positionC = A + vectorABD * 2;
            return positionC;
        }

        private bool CanHaveAnimation(string name) => name == "A";
    }
}


var camelCaseTokenizer = function (obj) {
    var previous = '';
    return obj.toString().trim().split(/[\s\-]+|(?=[A-Z])/).reduce(function(acc, cur) {
        var current = cur.toLowerCase();
        if(acc.length === 0) {
            previous = current;
            return acc.concat(current);
        }
        previous = previous.concat(current);
        return acc.concat([current, previous]);
    }, []);
}
lunr.tokenizer.registerFunction(camelCaseTokenizer, 'camelCaseTokenizer')
var searchModule = function() {
    var idMap = [];
    function y(e) { 
        idMap.push(e); 
    }
    var idx = lunr(function() {
        this.field('title', { boost: 10 });
        this.field('content');
        this.field('description', { boost: 5 });
        this.field('tags', { boost: 50 });
        this.ref('id');
        this.tokenizer(camelCaseTokenizer);

        this.pipeline.remove(lunr.stopWordFilter);
        this.pipeline.remove(lunr.stemmer);
    });
    function a(e) { 
        idx.add(e); 
    }

    a({
        id:0,
        title:"ILambdaDistribution",
        content:"ILambdaDistribution",
        description:'',
        tags:''
    });

    a({
        id:1,
        title:"ALFGenerator",
        content:"ALFGenerator",
        description:'',
        tags:''
    });

    a({
        id:2,
        title:"INuDistribution",
        content:"INuDistribution",
        description:'',
        tags:''
    });

    a({
        id:3,
        title:"FisherTippettDistribution",
        content:"FisherTippettDistribution",
        description:'',
        tags:''
    });

    a({
        id:4,
        title:"LogisticDistribution",
        content:"LogisticDistribution",
        description:'',
        tags:''
    });

    a({
        id:5,
        title:"PoissonDistribution",
        content:"PoissonDistribution",
        description:'',
        tags:''
    });

    a({
        id:6,
        title:"TMath",
        content:"TMath",
        description:'',
        tags:''
    });

    a({
        id:7,
        title:"ParetoDistribution",
        content:"ParetoDistribution",
        description:'',
        tags:''
    });

    a({
        id:8,
        title:"BetaDistribution",
        content:"BetaDistribution",
        description:'',
        tags:''
    });

    a({
        id:9,
        title:"BetaPrimeDistribution",
        content:"BetaPrimeDistribution",
        description:'',
        tags:''
    });

    a({
        id:10,
        title:"IWeightsDistribution",
        content:"IWeightsDistribution",
        description:'',
        tags:''
    });

    a({
        id:11,
        title:"ISigmaDistribution",
        content:"ISigmaDistribution",
        description:'',
        tags:''
    });

    a({
        id:12,
        title:"Extensions",
        content:"Extensions",
        description:'',
        tags:''
    });

    a({
        id:13,
        title:"IDistribution",
        content:"IDistribution",
        description:'',
        tags:''
    });

    a({
        id:14,
        title:"NR Generator",
        content:"NR Generator",
        description:'',
        tags:''
    });

    a({
        id:15,
        title:"LognormalDistribution",
        content:"LognormalDistribution",
        description:'',
        tags:''
    });

    a({
        id:16,
        title:"TriangularDistribution",
        content:"TriangularDistribution",
        description:'',
        tags:''
    });

    a({
        id:17,
        title:"IGammaDistribution",
        content:"IGammaDistribution",
        description:'',
        tags:''
    });

    a({
        id:18,
        title:"IGenerator",
        content:"IGenerator",
        description:'',
        tags:''
    });

    a({
        id:19,
        title:"ChiDistribution",
        content:"ChiDistribution",
        description:'',
        tags:''
    });

    a({
        id:20,
        title:"GammaDistribution",
        content:"GammaDistribution",
        description:'',
        tags:''
    });

    a({
        id:21,
        title:"MT Generator",
        content:"MT Generator",
        description:'',
        tags:''
    });

    a({
        id:22,
        title:"ErlangDistribution",
        content:"ErlangDistribution",
        description:'',
        tags:''
    });

    a({
        id:23,
        title:"FisherSnedecorDistribution",
        content:"FisherSnedecorDistribution",
        description:'',
        tags:''
    });

    a({
        id:24,
        title:"WeibullDistribution",
        content:"WeibullDistribution",
        description:'',
        tags:''
    });

    a({
        id:25,
        title:"TRandom",
        content:"TRandom",
        description:'',
        tags:''
    });

    a({
        id:26,
        title:"NormalDistribution",
        content:"NormalDistribution",
        description:'',
        tags:''
    });

    a({
        id:27,
        title:"RayleighDistribution",
        content:"RayleighDistribution",
        description:'',
        tags:''
    });

    a({
        id:28,
        title:"XorShift Generator",
        content:"XorShift Generator",
        description:'',
        tags:''
    });

    a({
        id:29,
        title:"StandardGenerator",
        content:"StandardGenerator",
        description:'',
        tags:''
    });

    a({
        id:30,
        title:"LaplaceDistribution",
        content:"LaplaceDistribution",
        description:'',
        tags:''
    });

    a({
        id:31,
        title:"IBetaDistribution",
        content:"IBetaDistribution",
        description:'',
        tags:''
    });

    a({
        id:32,
        title:"ContinuousUniformDistribution",
        content:"ContinuousUniformDistribution",
        description:'',
        tags:''
    });

    a({
        id:33,
        title:"IAlphaDistribution",
        content:"IAlphaDistribution",
        description:'',
        tags:''
    });

    a({
        id:34,
        title:"GeometricDistribution",
        content:"GeometricDistribution",
        description:'',
        tags:''
    });

    a({
        id:35,
        title:"IMuDistribution",
        content:"IMuDistribution",
        description:'',
        tags:''
    });

    a({
        id:36,
        title:"NR Generator",
        content:"NR Generator",
        description:'',
        tags:''
    });

    a({
        id:37,
        title:"ChiSquareDistribution",
        content:"ChiSquareDistribution",
        description:'',
        tags:''
    });

    a({
        id:38,
        title:"CategoricalDistribution",
        content:"CategoricalDistribution",
        description:'',
        tags:''
    });

    a({
        id:39,
        title:"BernoulliDistribution",
        content:"BernoulliDistribution",
        description:'',
        tags:''
    });

    a({
        id:40,
        title:"AbstractDistribution",
        content:"AbstractDistribution",
        description:'',
        tags:''
    });

    a({
        id:41,
        title:"IThetaDistribution",
        content:"IThetaDistribution",
        description:'',
        tags:''
    });

    a({
        id:42,
        title:"DiscreteUniformDistribution",
        content:"DiscreteUniformDistribution",
        description:'',
        tags:''
    });

    a({
        id:43,
        title:"IContinuousDistribution",
        content:"IContinuousDistribution",
        description:'',
        tags:''
    });

    a({
        id:44,
        title:"ExponentialDistribution",
        content:"ExponentialDistribution",
        description:'',
        tags:''
    });

    a({
        id:45,
        title:"AbstractGenerator",
        content:"AbstractGenerator",
        description:'',
        tags:''
    });

    a({
        id:46,
        title:"NR Generator",
        content:"NR Generator",
        description:'',
        tags:''
    });

    a({
        id:47,
        title:"StudentsTDistribution",
        content:"StudentsTDistribution",
        description:'',
        tags:''
    });

    a({
        id:48,
        title:"PowerDistribution",
        content:"PowerDistribution",
        description:'',
        tags:''
    });

    a({
        id:49,
        title:"BinomialDistribution",
        content:"BinomialDistribution",
        description:'',
        tags:''
    });

    a({
        id:50,
        title:"CauchyDistribution",
        content:"CauchyDistribution",
        description:'',
        tags:''
    });

    a({
        id:51,
        title:"IDiscreteDistribution",
        content:"IDiscreteDistribution",
        description:'',
        tags:''
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random/ILambdaDistribution_1',
        title:"ILambdaDistribution<TNum>",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Generators/ALFGenerator',
        title:"ALFGenerator",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random/INuDistribution_1',
        title:"INuDistribution<TNum>",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/FisherTippettDistribution',
        title:"FisherTippettDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/LogisticDistribution',
        title:"LogisticDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Discrete/PoissonDistribution',
        title:"PoissonDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random/TMath',
        title:"TMath",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/ParetoDistribution',
        title:"ParetoDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/BetaDistribution',
        title:"BetaDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/BetaPrimeDistribution',
        title:"BetaPrimeDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random/IWeightsDistribution_1',
        title:"IWeightsDistribution<T>",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random/ISigmaDistribution_1',
        title:"ISigmaDistribution<TNum>",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random/Extensions',
        title:"Extensions",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random/IDistribution',
        title:"IDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Generators/NR3Q2Generator',
        title:"NR3Q2Generator",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/LognormalDistribution',
        title:"LognormalDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/TriangularDistribution',
        title:"TriangularDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random/IGammaDistribution_1',
        title:"IGammaDistribution<TNum>",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random/IGenerator',
        title:"IGenerator",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/ChiDistribution',
        title:"ChiDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/GammaDistribution',
        title:"GammaDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Generators/MT19937Generator',
        title:"MT19937Generator",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/ErlangDistribution',
        title:"ErlangDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/FisherSnedecorDistribution',
        title:"FisherSnedecorDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/WeibullDistribution',
        title:"WeibullDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random/TRandom',
        title:"TRandom",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/NormalDistribution',
        title:"NormalDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/RayleighDistribution',
        title:"RayleighDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Generators/XorShift128Generator',
        title:"XorShift128Generator",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Generators/StandardGenerator',
        title:"StandardGenerator",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/LaplaceDistribution',
        title:"LaplaceDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random/IBetaDistribution_1',
        title:"IBetaDistribution<TNum>",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/ContinuousUniformDistribution',
        title:"ContinuousUniformDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random/IAlphaDistribution_1',
        title:"IAlphaDistribution<TNum>",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Discrete/GeometricDistribution',
        title:"GeometricDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random/IMuDistribution_1',
        title:"IMuDistribution<TNum>",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Generators/NR3Q1Generator',
        title:"NR3Q1Generator",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/ChiSquareDistribution',
        title:"ChiSquareDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Discrete/CategoricalDistribution',
        title:"CategoricalDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Discrete/BernoulliDistribution',
        title:"BernoulliDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions/AbstractDistribution',
        title:"AbstractDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random/IThetaDistribution_1',
        title:"IThetaDistribution<TNum>",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Discrete/DiscreteUniformDistribution',
        title:"DiscreteUniformDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random/IContinuousDistribution',
        title:"IContinuousDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/ExponentialDistribution',
        title:"ExponentialDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Generators/AbstractGenerator',
        title:"AbstractGenerator",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Generators/NR3Generator',
        title:"NR3Generator",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/StudentsTDistribution',
        title:"StudentsTDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/PowerDistribution',
        title:"PowerDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Discrete/BinomialDistribution',
        title:"BinomialDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random.Distributions.Continuous/CauchyDistribution',
        title:"CauchyDistribution",
        description:""
    });

    y({
        url:'/Troschuetz.Random/api/Troschuetz.Random/IDiscreteDistribution',
        title:"IDiscreteDistribution",
        description:""
    });

    return {
        search: function(q) {
            return idx.search(q).map(function(i) {
                return idMap[i.ref];
            });
        }
    };
}();

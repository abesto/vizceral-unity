using Model;
using UnityEngine;

public class ServiceEdge: MonoBehaviour {
    public LineRenderer lineRenderer;
    public ParticleSystem normalEmitter;
    public ParticleSystem warningEmitter;
    public ParticleSystem dangerEmitter;

    public void UpdateData(ServiceNode a, ServiceNode b, Metrics emitRate)
    {
        lineRenderer.SetPosition(0, a.transform.position);
        lineRenderer.SetPosition(1, b.transform.position);

        ConfigureEmitter(normalEmitter, emitRate.normal, b);
        ConfigureEmitter(warningEmitter, emitRate.warning, b);
        ConfigureEmitter(dangerEmitter, emitRate.danger, b);
    }

    private Vector3 from()
    {
        return lineRenderer.GetPosition(0);
    }

    private Vector3 to()
    {
        return lineRenderer.GetPosition(1);
    }


    private void ConfigureEmitter(ParticleSystem emitter, float emitRate, ServiceNode killTrigger)
    {
        var emission = emitter.emission;
        var main = emitter.main;
        var trigger = emitter.trigger;
        emitter.transform.position = from();
        emitter.transform.LookAt(to());
        emitter.transform.Rotate(90, 0, 0);
        emission.rateOverTime = emitRate;
        trigger.SetCollider(0, killTrigger);
     }
}
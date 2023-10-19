using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;

public abstract class ShaderSetting
{
    protected List<Material> materials = new List<Material>();

    public ShaderSetting(List<SkinnedMeshRenderer> meshes)
    {
        foreach (SkinnedMeshRenderer mesh in meshes)
            foreach (Material material in mesh.materials)
            {
                materials.Add(material);
            }
    }

    public abstract void RunShader();
    public abstract void Reset();
}

public class VMToon_Hit : ShaderSetting
{
    private TimerBuffer hit;

    public VMToon_Hit(List<SkinnedMeshRenderer> meshes, float hitTime) : base(meshes)
    {
        hit = new TimerBuffer(hitTime);
    }

    public override void Reset()
    {
        foreach (Material material in materials)
        {
            material.SetOverrideTag("RenderType", "");
            material.DisableKeyword("_HITTED");

            material.SetFloat("_DmgFactor", 0);
            material.SetFloat("_DmgEffMul", 1);
        }
    }

    public override void RunShader()
    {
        Timer.instance.TimerStop(hit);

        foreach (Material material in materials)
        {
            material.SetOverrideTag("RenderType", "Transparent");
            material.EnableKeyword("_HITTED");
        }

        Timer.instance.TimerStart(hit,
            OnFrame: () =>
            {
                foreach (Material material in materials)
                {
                    material.SetFloat("_DmgFactor", 1 - hit.timer / hit.time);
                    material.SetFloat("_DmgEffMul", 10);
                }
            },
            OnComplete: () => Reset());
    }
}

public class OmniShader_Hit : ShaderSetting
{
    private TimerBuffer hit;

    public OmniShader_Hit(List<SkinnedMeshRenderer> meshes, float hitTime) : base(meshes)
    {
        hit = new TimerBuffer(hitTime);
    }

    public override void Reset()
    {
        foreach (Material material in materials)
        {
            material.SetColor("_Color", Color.white);       
            
        }
    }

    public override void RunShader()
    {
        Timer.instance.TimerStop(hit);

        foreach (Material material in materials)
        {
            //material.SetOverrideTag("RenderType", "Transparent");
            //material.EnableKeyword("_HITTED");
            material.SetColor("_Color", Color.red);
        }

        Timer.instance.TimerStart(hit,
            OnFrame: () =>
            {
               // foreach (Material material in materials)
               // {
               //     material.SetFloat("_DmgFactor", 1 - hit.timer / hit.time);
               //     material.SetFloat("_DmgEffMul", 10);
               // }
            },
            OnComplete: () => Reset());
    }
}
public class ShaderFunction : Singleton<ShaderFunction>
{
    public Dictionary<Model, List<ShaderSetting>> shaderSettings = new Dictionary<Model, List<ShaderSetting>>();

    public void AllReset(Model model)
    {
        if (shaderSettings.ContainsKey(model))
        {
            foreach (var shaderSetting in shaderSettings[model])
                shaderSetting.Reset();
        }
    }
    public void Play_Hit(Model model, float hitTime)
    {
        ShaderSetting shaderSetting = GetShaderSetting(model, typeof(OmniShader_Hit));

        if (shaderSetting == null)
            shaderSettings[model].Add(shaderSetting = new OmniShader_Hit(model.meshes, hitTime));

        shaderSetting.RunShader();
    }

    public void Play_VMToon_Hit(Model model, float hitTime)
    {
        ShaderSetting shaderSetting = GetShaderSetting(model, typeof(VMToon_Hit));

        if (shaderSetting == null)
            shaderSettings[model].Add(shaderSetting = new VMToon_Hit(model.meshes, hitTime));

        shaderSetting.RunShader();
    }

    private ShaderSetting GetShaderSetting(Model model, Type checkType)
    {
        if (!shaderSettings.ContainsKey(model))
        {
            shaderSettings.Add(model, new List<ShaderSetting>());
        }

        return shaderSettings[model].Find(data => checkType.IsAssignableFrom(data.GetType()));
    }
}
